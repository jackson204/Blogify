// 文章詳情頁邏輯
class ArticleDetail {
    constructor() {
        this.articleId = null;
        this.currentArticle = null;
        this.init();
    }

    init() {
        this.getArticleIdFromURL();
        if (this.articleId) {
            this.loadArticle();
        } else {
            this.showError();
        }
        this.bindEvents();
    }

    getArticleIdFromURL() {
        const urlParams = new URLSearchParams(window.location.search);
        this.articleId = urlParams.get('id');
    }

    bindEvents() {
        // 點讚按鈕
        document.addEventListener('click', async (e) => {
            if (e.target.closest('#likeButton')) {
                e.preventDefault();
                await this.handleLike();
            }
        });

        // 分享按鈕
        document.getElementById('shareButton')?.addEventListener('click', () => {
            this.shareArticle();
        });

        // 列印按鈕
        document.getElementById('printButton')?.addEventListener('click', () => {
            this.printArticle();
        });

        // 滾動監聽（目錄高亮）
        window.addEventListener('scroll', Utils.debounce(() => {
            this.updateTOCHighlight();
        }, 100));
    }

    async loadArticle() {
        try {
            const result = await clientAPI.getArticle(this.articleId);
            
            if (result.success) {
                this.currentArticle = result.data;
                this.renderArticle(result.data);
                this.generateTableOfContents();
                this.loadSidebarContent();
                this.showContent();
            } else {
                this.showError();
            }
        } catch (error) {
            console.error('載入文章失敗:', error);
            this.showError();
        }
    }

    renderArticle(article) {
        // 更新頁面標題
        document.title = `${article.title} - MyBlog`;
        
        // 更新 meta 描述
        const metaDesc = document.querySelector('meta[name="description"]');
        if (metaDesc) {
            metaDesc.content = article.excerpt;
        }

        // 填充文章內容
        document.getElementById('articleCategory').textContent = article.category;
        document.getElementById('articleTitle').textContent = article.title;
        document.getElementById('articleAuthor').textContent = article.author;
        document.getElementById('articleDate').textContent = Utils.formatDate(article.publishDate);
        document.getElementById('articleViews').textContent = article.views;
        document.getElementById('articleReadTime').textContent = article.readTime;
        document.getElementById('likesCount').textContent = article.likes;
        
        // 設定圖片
        const articleImage = document.getElementById('articleImage');
        articleImage.src = article.image;
        articleImage.alt = article.title;

        // 設定標籤
        const tagsContainer = document.getElementById('articleTags');
        tagsContainer.innerHTML = article.tags.map(tag => 
            `<span class="tag">#${tag}</span>`
        ).join('');

        // 設定文章內容
        this.renderArticleContent(article.content);

        // 設定點讚按鈕狀態
        const likeButton = document.getElementById('likeButton');
        if (clientAPI.hasLiked(article.id)) {
            likeButton.classList.add('liked');
        }
    }

    renderArticleContent(content) {
        const articleText = document.getElementById('articleText');
        
        // 將 Markdown 風格的內容轉換為 HTML
        let htmlContent = content
            // 標題
            .replace(/^## (.*$)/gm, '<h2>$1</h2>')
            .replace(/^### (.*$)/gm, '<h3>$1</h3>')
            .replace(/^#### (.*$)/gm, '<h4>$1</h4>')
            // 程式碼區塊
            .replace(/```(\w+)?\n([\s\S]*?)```/g, '<pre><code>$2</code></pre>')
            // 行內程式碼
            .replace(/`([^`]+)`/g, '<code>$1</code>')
            // 粗體
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            // 斜體
            .replace(/\*(.*?)\*/g, '<em>$1</em>')
            // 連結
            .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2" target="_blank">$1</a>')
            // 段落
            .replace(/\n\n/g, '</p><p>')
            // 列表項目
            .replace(/^- (.*$)/gm, '<li>$1</li>')
            // 包裝列表
            .replace(/(<li>.*<\/li>)/s, '<ul>$1</ul>')
            // 包裝段落
            .replace(/^(?!<[hup]|<\/[hup]|<li|<\/li|<ul|<\/ul|<pre|<\/pre)(.+)$/gm, '<p>$1</p>');

        articleText.innerHTML = htmlContent;
    }

    generateTableOfContents() {
        const tocContainer = document.getElementById('tableOfContents');
        const headings = document.querySelectorAll('#articleText h2, #articleText h3, #articleText h4');
        
        if (headings.length === 0) {
            tocContainer.innerHTML = '<p class="text-muted">此文章沒有目錄</p>';
            return;
        }

        let tocHTML = '<ul>';
        headings.forEach((heading, index) => {
            const id = `heading-${index}`;
            heading.id = id;
            
            const level = heading.tagName.toLowerCase();
            const text = heading.textContent;
            
            tocHTML += `
                <li>
                    <a href="#${id}" class="toc-${level}" data-target="${id}">
                        ${text}
                    </a>
                </li>
            `;
        });
        tocHTML += '</ul>';
        
        tocContainer.innerHTML = tocHTML;

        // 綁定目錄點擊事件
        tocContainer.addEventListener('click', (e) => {
            if (e.target.tagName === 'A') {
                e.preventDefault();
                const targetId = e.target.dataset.target;
                const targetElement = document.getElementById(targetId);
                if (targetElement) {
                    targetElement.scrollIntoView({ 
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    }

    updateTOCHighlight() {
        const headings = document.querySelectorAll('#articleText h2, #articleText h3, #articleText h4');
        const tocLinks = document.querySelectorAll('.toc a');
        
        let activeHeading = null;
        const scrollTop = window.pageYOffset;
        
        // 找到當前可見的標題
        headings.forEach(heading => {
            const rect = heading.getBoundingClientRect();
            if (rect.top <= 100) {
                activeHeading = heading;
            }
        });

        // 更新目錄高亮
        tocLinks.forEach(link => {
            link.classList.remove('active');
            if (activeHeading && link.dataset.target === activeHeading.id) {
                link.classList.add('active');
            }
        });
    }

    async loadSidebarContent() {
        try {
            // 並行載入側邊欄內容
            const [latestResult] = await Promise.all([
                clientAPI.getLatestArticles(5)
            ]);

            if (latestResult.success) {
                this.renderLatestArticles(latestResult.data);
            }

            // 載入相關文章（基於分類）
            this.loadRelatedArticles();

        } catch (error) {
            console.error('載入側邊欄內容失敗:', error);
        }
    }

    async loadRelatedArticles() {
        try {
            const result = await clientAPI.getArticles({ 
                category: this.currentArticle.category,
                limit: 5 
            });
            
            if (result.success) {
                // 過濾掉當前文章
                const relatedArticles = result.data.articles.filter(
                    article => article.id !== this.currentArticle.id
                ).slice(0, 4);
                
                this.renderRelatedArticles(relatedArticles);
            }
        } catch (error) {
            console.error('載入相關文章失敗:', error);
        }
    }

    renderRelatedArticles(articles) {
        const container = document.getElementById('relatedArticles');
        
        if (articles.length === 0) {
            container.innerHTML = '<p class="text-muted">暫無相關文章</p>';
            return;
        }

        container.innerHTML = `
            <div class="related-list">
                ${articles.map(article => `
                    <div class="related-item">
                        <a href="article.html?id=${article.id}" class="related-title">
                            ${article.title}
                        </a>
                        <div class="related-meta">
                            <span>${article.author}</span> • 
                            <span>${Utils.formatDate(article.publishDate)}</span>
                        </div>
                    </div>
                `).join('')}
            </div>
        `;
    }

    renderLatestArticles(articles) {
        const container = document.getElementById('latestArticles');
        
        container.innerHTML = `
            <div class="sidebar-section">
                <h3 class="sidebar-title">最新文章</h3>
                <div class="latest-list">
                    ${articles.map(article => `
                        <div class="latest-item">
                            <a href="article.html?id=${article.id}" class="latest-title">
                                ${article.title}
                            </a>
                            <div class="latest-meta">
                                <span class="latest-author">${article.author}</span>
                                <span class="latest-date">${Utils.formatDate(article.publishDate)}</span>
                            </div>
                        </div>
                    `).join('')}
                </div>
            </div>
        `;
    }

    async handleLike() {
        if (!this.currentArticle) return;

        try {
            const result = await clientAPI.likeArticle(this.currentArticle.id);
            
            if (result.success) {
                // 更新按鈕狀態
                const likeButton = document.getElementById('likeButton');
                const likesCount = document.getElementById('likesCount');
                
                likeButton.classList.add('liked');
                likesCount.textContent = result.data.likes;
                
                Utils.showNotification(result.data.message, 'success');
            } else {
                Utils.showNotification(result.error, 'warning');
            }
        } catch (error) {
            console.error('點讚失敗:', error);
            Utils.showNotification('點讚失敗，請稍後再試', 'error');
        }
    }

    shareArticle() {
        if (!this.currentArticle) return;

        const shareData = {
            title: this.currentArticle.title,
            text: this.currentArticle.excerpt,
            url: window.location.href
        };

        if (navigator.share) {
            // 使用原生分享 API
            navigator.share(shareData).catch(err => {
                console.log('分享取消:', err);
            });
        } else {
            // 複製連結到剪貼簿
            navigator.clipboard.writeText(window.location.href).then(() => {
                Utils.showNotification('文章連結已複製到剪貼簿', 'success');
            }).catch(() => {
                // 降級方案
                const textArea = document.createElement('textarea');
                textArea.value = window.location.href;
                document.body.appendChild(textArea);
                textArea.select();
                document.execCommand('copy');
                document.body.removeChild(textArea);
                Utils.showNotification('文章連結已複製到剪貼簿', 'success');
            });
        }
    }

    printArticle() {
        // 建立列印樣式
        const printStyles = `
            <style>
                @media print {
                    body * { visibility: hidden; }
                    .article-main, .article-main * { visibility: visible; }
                    .article-main { position: absolute; left: 0; top: 0; width: 100%; }
                    .article-footer, .article-actions { display: none; }
                    .article-header { border-bottom: 2px solid #333; }
                    .article-title-main { font-size: 24pt; }
                    .article-text { font-size: 12pt; line-height: 1.6; }
                    .article-text h2 { font-size: 18pt; }
                    .article-text h3 { font-size: 16pt; }
                    .article-text h4 { font-size: 14pt; }
                }
            </style>
        `;
        
        // 添加列印樣式到頁面
        const styleElement = document.createElement('div');
        styleElement.innerHTML = printStyles;
        document.head.appendChild(styleElement);
        
        // 執行列印
        window.print();
        
        // 移除列印樣式
        setTimeout(() => {
            document.head.removeChild(styleElement);
        }, 1000);
    }

    showContent() {
        document.getElementById('loadingState').classList.add('d-none');
        document.getElementById('errorState').classList.add('d-none');
        document.getElementById('articleContent').classList.remove('d-none');
    }

    showError() {
        document.getElementById('loadingState').classList.add('d-none');
        document.getElementById('articleContent').classList.add('d-none');
        document.getElementById('errorState').classList.remove('d-none');
    }
}

// 頁面載入完成後初始化
document.addEventListener('DOMContentLoaded', () => {
    window.articleDetail = new ArticleDetail();
}); 