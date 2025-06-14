// 客戶端主要邏輯
class BlogClient {
    constructor() {
        this.currentPage = 1;
        this.currentCategory = '';
        this.currentSearch = '';
        this.currentSort = 'date';
        this.isLoading = false;
        
        this.init();
    }

    async init() {
        this.bindEvents();
        await this.loadInitialData();
    }

    bindEvents() {
        // 搜尋功能
        const searchInput = document.getElementById('searchInput');
        const searchBtn = document.getElementById('searchBtn');
        
        if (searchInput && searchBtn) {
            const debouncedSearch = Utils.debounce(() => {
                this.handleSearch();
            }, 500);
            
            searchInput.addEventListener('input', debouncedSearch);
            searchBtn.addEventListener('click', () => this.handleSearch());
            
            searchInput.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') {
                    this.handleSearch();
                }
            });
        }

        // 分類篩選
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('category-filter')) {
                e.preventDefault();
                const category = e.target.dataset.category;
                this.handleCategoryFilter(category);
            }
        });

        // 排序
        const sortSelect = document.getElementById('sortSelect');
        if (sortSelect) {
            sortSelect.addEventListener('change', (e) => {
                this.currentSort = e.target.value;
                this.currentPage = 1;
                this.loadArticles();
            });
        }

        // 分頁
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('page-btn')) {
                e.preventDefault();
                const page = parseInt(e.target.dataset.page);
                if (page && page !== this.currentPage) {
                    this.currentPage = page;
                    this.loadArticles();
                }
            }
        });

        // 點讚功能
        document.addEventListener('click', async (e) => {
            if (e.target.classList.contains('like-btn')) {
                e.preventDefault();
                const articleId = parseInt(e.target.dataset.id);
                await this.handleLike(articleId);
            }
        });
    }

    async loadInitialData() {
        try {
            // 並行載入資料
            const [articlesResult, categoriesResult, popularResult, latestResult] = await Promise.all([
                clientAPI.getArticles({ page: 1, sort: this.currentSort }),
                clientAPI.getCategories(),
                clientAPI.getPopularArticles(),
                clientAPI.getLatestArticles()
            ]);

            if (articlesResult.success) {
                this.renderArticles(articlesResult.data.articles);
                this.renderPagination(articlesResult.data.pagination);
            }

            if (categoriesResult.success) {
                this.renderCategories(categoriesResult.data);
            }

            if (popularResult.success) {
                this.renderPopularArticles(popularResult.data);
            }

            if (latestResult.success) {
                this.renderLatestArticles(latestResult.data);
            }

        } catch (error) {
            console.error('載入初始資料失敗:', error);
            Utils.showNotification('載入資料失敗，請重新整理頁面', 'error');
        }
    }

    async loadArticles() {
        if (this.isLoading) return;
        
        this.isLoading = true;
        const articlesContainer = document.getElementById('articlesContainer');
        const loader = Utils.showLoading(articlesContainer);

        try {
            const params = {
                page: this.currentPage,
                sort: this.currentSort
            };

            if (this.currentCategory) {
                params.category = this.currentCategory;
            }

            if (this.currentSearch) {
                params.search = this.currentSearch;
            }

            const result = await clientAPI.getArticles(params);

            if (result.success) {
                this.renderArticles(result.data.articles);
                this.renderPagination(result.data.pagination);
                
                if (result.data.articles.length === 0) {
                    this.renderNoResults();
                }
            } else {
                Utils.showNotification('載入文章失敗', 'error');
            }

        } catch (error) {
            console.error('載入文章失敗:', error);
            Utils.showNotification('載入文章失敗', 'error');
        } finally {
            Utils.hideLoading(loader);
            this.isLoading = false;
        }
    }

    renderArticles(articles) {
        const container = document.getElementById('articlesContainer');
        if (!container) return;

        if (articles.length === 0) {
            this.renderNoResults();
            return;
        }

        container.innerHTML = articles.map(article => `
            <article class="card article-card mb-4">
                <div class="article-image">
                    <img src="${article.image}" alt="${article.title}" loading="lazy">
                </div>
                <div class="card-body">
                    <div class="article-meta mb-2">
                        <span class="category-tag">${article.category}</span>
                        <span class="publish-date">${Utils.formatDate(article.publishDate)}</span>
                    </div>
                    <h2 class="article-title">
                        <a href="article.html?id=${article.id}">${article.title}</a>
                    </h2>
                    <p class="article-excerpt">${article.excerpt}</p>
                    <div class="article-tags mb-3">
                        ${article.tags.map(tag => `<span class="tag">#${tag}</span>`).join('')}
                    </div>
                    <div class="article-footer d-flex justify-content-between align-items-center">
                        <div class="article-stats">
                            <span class="stat">
                                <i class="icon-eye"></i> ${article.views}
                            </span>
                            <span class="stat">
                                <i class="icon-clock"></i> ${article.readTime} 分鐘
                            </span>
                            <span class="stat">
                                <i class="icon-user"></i> ${article.author}
                            </span>
                        </div>
                        <div class="article-actions">
                            <button class="like-btn ${clientAPI.hasLiked(article.id) ? 'liked' : ''}" 
                                    data-id="${article.id}">
                                <i class="icon-heart"></i> ${article.likes}
                            </button>
                        </div>
                    </div>
                </div>
            </article>
        `).join('');
    }

    renderNoResults() {
        const container = document.getElementById('articlesContainer');
        if (!container) return;

        container.innerHTML = `
            <div class="no-results text-center p-5">
                <div class="no-results-icon mb-3">
                    <i class="icon-search" style="font-size: 4rem; color: #ccc;"></i>
                </div>
                <h3>沒有找到相關文章</h3>
                <p class="text-muted">試試調整搜尋條件或瀏覽其他分類</p>
                <button class="btn btn-primary mt-3" onclick="blogClient.clearFilters()">
                    清除篩選條件
                </button>
            </div>
        `;
    }

    renderCategories(categories) {
        const container = document.getElementById('categoriesContainer');
        if (!container) return;

        container.innerHTML = `
            <div class="categories-list">
                <a href="#" class="category-filter ${!this.currentCategory ? 'active' : ''}" 
                   data-category="">
                    全部分類
                </a>
                ${categories.map(category => `
                    <a href="#" class="category-filter ${this.currentCategory === category.name ? 'active' : ''}" 
                       data-category="${category.name}">
                        ${category.name} (${category.count})
                    </a>
                `).join('')}
            </div>
        `;
    }

    renderPopularArticles(articles) {
        const container = document.getElementById('popularArticles');
        if (!container) return;

        container.innerHTML = `
            <div class="sidebar-section">
                <h3 class="sidebar-title">熱門文章</h3>
                <div class="popular-list">
                    ${articles.map((article, index) => `
                        <div class="popular-item">
                            <span class="popular-rank">${index + 1}</span>
                            <div class="popular-content">
                                <a href="article.html?id=${article.id}" class="popular-title">
                                    ${article.title}
                                </a>
                                <div class="popular-stats">
                                    <span><i class="icon-eye"></i> ${article.views}</span>
                                </div>
                            </div>
                        </div>
                    `).join('')}
                </div>
            </div>
        `;
    }

    renderLatestArticles(articles) {
        const container = document.getElementById('latestArticles');
        if (!container) return;

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

    renderPagination(pagination) {
        const container = document.getElementById('paginationContainer');
        if (!container || pagination.total <= 1) {
            if (container) container.innerHTML = '';
            return;
        }

        const { current, total } = pagination;
        let paginationHTML = '<div class="pagination">';

        // 上一頁
        if (current > 1) {
            paginationHTML += `
                <a href="#" class="page-btn" data-page="${current - 1}">
                    <i class="icon-chevron-left"></i> 上一頁
                </a>
            `;
        }

        // 頁碼
        const startPage = Math.max(1, current - 2);
        const endPage = Math.min(total, current + 2);

        if (startPage > 1) {
            paginationHTML += `<a href="#" class="page-btn" data-page="1">1</a>`;
            if (startPage > 2) {
                paginationHTML += `<span class="page-ellipsis">...</span>`;
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            paginationHTML += `
                <a href="#" class="page-btn ${i === current ? 'active' : ''}" data-page="${i}">
                    ${i}
                </a>
            `;
        }

        if (endPage < total) {
            if (endPage < total - 1) {
                paginationHTML += `<span class="page-ellipsis">...</span>`;
            }
            paginationHTML += `<a href="#" class="page-btn" data-page="${total}">${total}</a>`;
        }

        // 下一頁
        if (current < total) {
            paginationHTML += `
                <a href="#" class="page-btn" data-page="${current + 1}">
                    下一頁 <i class="icon-chevron-right"></i>
                </a>
            `;
        }

        paginationHTML += '</div>';
        container.innerHTML = paginationHTML;
    }

    async handleSearch() {
        const searchInput = document.getElementById('searchInput');
        if (!searchInput) return;

        this.currentSearch = searchInput.value.trim();
        this.currentPage = 1;
        await this.loadArticles();
    }

    async handleCategoryFilter(category) {
        this.currentCategory = category;
        this.currentPage = 1;
        
        // 更新分類按鈕狀態
        document.querySelectorAll('.category-filter').forEach(btn => {
            btn.classList.remove('active');
        });
        
        const activeBtn = document.querySelector(`[data-category="${category}"]`);
        if (activeBtn) {
            activeBtn.classList.add('active');
        }

        await this.loadArticles();
    }

    async handleLike(articleId) {
        try {
            const result = await clientAPI.likeArticle(articleId);
            
            if (result.success) {
                // 更新按鈕狀態
                const likeBtn = document.querySelector(`[data-id="${articleId}"]`);
                if (likeBtn) {
                    likeBtn.classList.add('liked');
                    likeBtn.innerHTML = `<i class="icon-heart"></i> ${result.data.likes}`;
                }
                
                Utils.showNotification(result.data.message, 'success');
            } else {
                Utils.showNotification(result.error, 'warning');
            }
        } catch (error) {
            console.error('點讚失敗:', error);
            Utils.showNotification('點讚失敗，請稍後再試', 'error');
        }
    }

    clearFilters() {
        this.currentCategory = '';
        this.currentSearch = '';
        this.currentPage = 1;
        
        // 清除搜尋框
        const searchInput = document.getElementById('searchInput');
        if (searchInput) {
            searchInput.value = '';
        }
        
        // 重置分類按鈕
        document.querySelectorAll('.category-filter').forEach(btn => {
            btn.classList.remove('active');
        });
        
        const allCategoryBtn = document.querySelector('[data-category=""]');
        if (allCategoryBtn) {
            allCategoryBtn.classList.add('active');
        }
        
        this.loadArticles();
    }
}

// 頁面載入完成後初始化
document.addEventListener('DOMContentLoaded', () => {
    window.blogClient = new BlogClient();
}); 