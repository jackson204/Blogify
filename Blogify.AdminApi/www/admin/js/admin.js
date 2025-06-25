// 管理端主要邏輯
class AdminManager {
    constructor() {
        this.currentPage = 1;
        this.currentFilters = {
            search: '',
            status: '',
            category: '',
            sort: 'updatedAt'
        };
        this.selectedArticles = new Set();
        this.isEditing = false;
        this.editingArticleId = null;
        
        this.init();
    }

    async init() {
        // 檢查當前頁面
        const currentPath = window.location.pathname;
        
        if (currentPath.includes('index.html') || currentPath.endsWith('/admin/')) {
            await this.initDashboard();
        } else if (currentPath.includes('articles.html') || currentPath.includes('/admin/Article/')) {
            await this.initArticlesPage();
        } else if (currentPath.includes('categories.html')) {
            await this.initCategoriesPage();
        }
        // 如果是其他頁面，只綁定全域事件，不執行特定初始化
        
        this.bindGlobalEvents();
    }

    // 初始化儀表板
    async initDashboard() {
        try {
            await this.loadDashboardData();
        } catch (error) {
            console.error('載入儀表板失敗:', error);
            Utils.showNotification('載入儀表板資料失敗', 'error');
        }
    }

    // 載入儀表板資料
    async loadDashboardData() {
        const [statsResult, articlesResult] = await Promise.all([
            adminAPI.getStats(),
            adminAPI.getArticles({ limit: 5, sort: 'updatedAt' })
        ]);

        if (statsResult.success) {
            this.renderStats(statsResult.data);
        }

        if (articlesResult.success) {
            this.renderRecentArticles(articlesResult.data.articles);
        }

        // 更新最後登入時間
        this.updateLastLoginTime();
    }

    // 渲染統計資料
    renderStats(stats) {
        const container = document.getElementById('statsContainer');
        if (!container) return;

        container.innerHTML = `
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">總文章數</span>
                    <div class="stat-icon">📝</div>
                </div>
                <div class="stat-value">${stats.totalArticles}</div>
                <div class="stat-change">+${stats.publishedArticles} 已發布</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">總瀏覽數</span>
                    <div class="stat-icon">👁️</div>
                </div>
                <div class="stat-value">${stats.totalViews.toLocaleString()}</div>
                <div class="stat-change">累計瀏覽</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">總點讚數</span>
                    <div class="stat-icon">❤️</div>
                </div>
                <div class="stat-value">${stats.totalLikes}</div>
                <div class="stat-change">讀者喜愛</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">草稿文章</span>
                    <div class="stat-icon">📄</div>
                </div>
                <div class="stat-value">${stats.draftArticles}</div>
                <div class="stat-change">待發布</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">精選文章</span>
                    <div class="stat-icon">⭐</div>
                </div>
                <div class="stat-value">${stats.featuredArticles}</div>
                <div class="stat-change">推薦內容</div>
            </div>
        `;
    }

    // 渲染最新文章
    renderRecentArticles(articles) {
        const container = document.getElementById('recentArticles');
        if (!container) return;

        if (articles.length === 0) {
            container.innerHTML = '<p class="text-muted text-center">暫無文章</p>';
            return;
        }

        container.innerHTML = `
            <div class="recent-articles-list">
                ${articles.map(article => `
                    <div class="recent-article-item">
                        <div class="article-info">
                            <h4 class="article-title">
                                <a href="/admin/Article/Edit/${article.id}">${article.title}</a>
                            </h4>
                            <div class="article-meta">
                                <span class="status-badge status-${article.status}">${this.getStatusText(article.status)}</span>
                                <span class="article-author">${article.author}</span>
                                <span class="article-date">${Utils.formatDate(article.updatedAt)}</span>
                            </div>
                        </div>
                        <div class="article-stats">
                            <span class="stat-item">👁️ ${article.views}</span>
                            <span class="stat-item">❤️ ${article.likes}</span>
                        </div>
                    </div>
                `).join('')}
            </div>
        `;
    }

    // 更新最後登入時間
    updateLastLoginTime() {
        const lastLoginElement = document.getElementById('lastLoginTime');
        if (lastLoginElement) {
            lastLoginElement.textContent = Utils.formatDate(new Date());
        }
    }

    // 初始化文章管理頁面
    async initArticlesPage() {
        try {
            await this.loadCategories();
            await this.loadArticles();
            this.bindArticlesEvents();
            this.checkURLParams();
        } catch (error) {
            console.error('初始化文章頁面失敗:', error);
            Utils.showNotification('載入文章管理頁面失敗', 'error');
        }
    }

    // 檢查 URL 參數
    checkURLParams() {
        const urlParams = new URLSearchParams(window.location.search);
        const action = urlParams.get('action');
        const editId = urlParams.get('edit');
        const status = urlParams.get('status');

        if (action === 'new') {
            this.showEditView();
        } else if (editId) {
            this.editArticle(parseInt(editId));
        } else if (status) {
            const statusFilter = document.getElementById('statusFilter');
            if (statusFilter) {
                statusFilter.value = status;
                this.currentFilters.status = status;
                this.loadArticles();
            }
        }
    }

    // 載入分類資料
    async loadCategories() {
        try {
            const result = await adminAPI.getCategories();
            if (result.success) {
                this.populateCategorySelects(result.data);
            }
        } catch (error) {
            console.error('載入分類失敗:', error);
        }
    }

    // 填充分類選擇器
    populateCategorySelects(categories) {
        const selectors = ['categoryFilter', 'articleCategory'];
        
        selectors.forEach(selectorId => {
            const select = document.getElementById(selectorId);
            if (select) {
                const isFilter = selectorId === 'categoryFilter';
                const currentValue = select.value;
                
                select.innerHTML = isFilter ? '<option value="">所有分類</option>' : '<option value="">請選擇分類</option>';
                
                categories.forEach(category => {
                    const option = document.createElement('option');
                    option.value = category.name;
                    option.textContent = `${category.name}${isFilter ? ` (${category.count})` : ''}`;
                    select.appendChild(option);
                });
                
                if (currentValue) {
                    select.value = currentValue;
                }
            }
        });
    }

    // 綁定文章管理事件
    bindArticlesEvents() {
        // 新增文章按鈕
        const newArticleBtn = document.getElementById('newArticleBtn');
        if (newArticleBtn) {
            newArticleBtn.addEventListener('click', () => this.showEditView());
        }

        // 取消編輯按鈕
        const cancelEditBtn = document.getElementById('cancelEditBtn');
        if (cancelEditBtn) {
            cancelEditBtn.addEventListener('click', () => this.showListView());
        }

        // 文章表單提交
        const articleForm = document.getElementById('articleForm');
        if (articleForm) {
            articleForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.saveArticle();
            });
        }

        // 篩選器事件
        this.bindFilterEvents();

        // 批量操作事件
        this.bindBatchEvents();

        // 編輯器工具列事件
        this.bindEditorEvents();
    }

    // 綁定篩選器事件
    bindFilterEvents() {
        const searchInput = document.getElementById('searchInput');
        const statusFilter = document.getElementById('statusFilter');
        const categoryFilter = document.getElementById('categoryFilter');
        const sortFilter = document.getElementById('sortFilter');

        if (searchInput) {
            const debouncedSearch = Utils.debounce(() => {
                this.currentFilters.search = searchInput.value.trim();
                this.currentPage = 1;
                this.loadArticles();
            }, 500);
            searchInput.addEventListener('input', debouncedSearch);
        }

        [statusFilter, categoryFilter, sortFilter].forEach(filter => {
            if (filter) {
                filter.addEventListener('change', () => {
                    this.currentFilters.status = statusFilter?.value || '';
                    this.currentFilters.category = categoryFilter?.value || '';
                    this.currentFilters.sort = sortFilter?.value || 'updatedAt';
                    this.currentPage = 1;
                    this.loadArticles();
                });
            }
        });
    }

    // 綁定批量操作事件
    bindBatchEvents() {
        // 全選/取消全選
        const selectAllCheckbox = document.getElementById('selectAll');
        if (selectAllCheckbox) {
            selectAllCheckbox.addEventListener('change', (e) => {
                this.toggleSelectAll(e.target.checked);
            });
        }

        // 批量操作按鈕
        const batchButtons = document.querySelectorAll('#batchActions button[data-action]');
        batchButtons.forEach(btn => {
            btn.addEventListener('click', (e) => {
                const action = e.target.dataset.action;
                this.handleBatchAction(action);
            });
        });
    }

    // 綁定編輯器事件
    bindEditorEvents() {
        const editorButtons = document.querySelectorAll('.editor-btn[data-action]');
        editorButtons.forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.preventDefault();
                const action = e.target.dataset.action;
                this.handleEditorAction(action);
            });
        });
    }

    // 載入文章列表
    async loadArticles() {
        const tableBody = document.getElementById('articlesTableBody');
        if (!tableBody) return;

        // 顯示載入狀態
        tableBody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center p-4">
                    <div class="spinner"></div>
                    <p class="mt-3">載入中...</p>
                </td>
            </tr>
        `;

        try {
            const params = {
                page: this.currentPage,
                limit: 10,
                ...this.currentFilters
            };

            const result = await adminAPI.getArticles(params);

            if (result.success) {
                this.renderArticlesTable(result.data.articles);
                this.renderPagination(result.data.pagination);
            } else {
                throw new Error(result.error);
            }
        } catch (error) {
            console.error('載入文章失敗:', error);
            tableBody.innerHTML = `
                <tr>
                    <td colspan="8" class="text-center p-4">
                        <p class="text-danger">載入失敗：${error.message}</p>
                        <button class="btn btn-primary btn-sm mt-2" onclick="adminManager.loadArticles()">重試</button>
                    </td>
                </tr>
            `;
        }
    }

    // 渲染文章表格
    renderArticlesTable(articles) {
        const tableBody = document.getElementById('articlesTableBody');
        if (!tableBody) return;

        if (articles.length === 0) {
            tableBody.innerHTML = `
                <tr>
                    <td colspan="8" class="text-center p-4">
                        <p class="text-muted">沒有找到符合條件的文章</p>
                    </td>
                </tr>
            `;
            return;
        }

        tableBody.innerHTML = articles.map(article => `
            <tr>
                <td>
                    <input type="checkbox" class="article-checkbox" value="${article.id}" 
                           ${this.selectedArticles.has(article.id) ? 'checked' : ''}>
                </td>
                <td>
                    <div class="article-title-cell">
                        <strong>${article.title}</strong>
                        ${article.featured ? '<span class="status-badge status-featured">精選</span>' : ''}
                    </div>
                </td>
                <td>${article.author}</td>
                <td>${article.category}</td>
                <td>
                    <span class="status-badge status-${article.status}">
                        ${this.getStatusText(article.status)}
                    </span>
                </td>
                <td>${article.views}</td>
                <td>${Utils.formatDate(article.updatedAt)}</td>
                <td>
                    <div class="table-actions">
                        <button class="action-btn btn-view" onclick="window.open('../client/article.html?id=${article.id}', '_blank')" title="預覽">
                            <i class="icon-view"></i>
                        </button>
                        <button class="action-btn btn-edit" onclick="adminManager.editArticle(${article.id})" title="編輯">
                            <i class="icon-edit"></i>
                        </button>
                        <button class="action-btn btn-delete" onclick="adminManager.deleteArticle(${article.id})" title="刪除">
                            <i class="icon-delete"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');

        // 重新綁定複選框事件
        this.bindCheckboxEvents();
    }

    // 綁定複選框事件
    bindCheckboxEvents() {
        const checkboxes = document.querySelectorAll('.article-checkbox');
        checkboxes.forEach(checkbox => {
            checkbox.addEventListener('change', (e) => {
                const articleId = parseInt(e.target.value);
                if (e.target.checked) {
                    this.selectedArticles.add(articleId);
                } else {
                    this.selectedArticles.delete(articleId);
                }
                this.updateBatchActions();
            });
        });
    }

    // 更新批量操作顯示
    updateBatchActions() {
        const batchActions = document.getElementById('batchActions');
        const selectedCount = document.getElementById('selectedCount');
        const selectAllCheckbox = document.getElementById('selectAll');

        if (batchActions && selectedCount) {
            const count = this.selectedArticles.size;
            selectedCount.textContent = count;
            
            if (count > 0) {
                batchActions.classList.remove('d-none');
            } else {
                batchActions.classList.add('d-none');
            }
        }

        // 更新全選複選框狀態
        if (selectAllCheckbox) {
            const checkboxes = document.querySelectorAll('.article-checkbox');
            const checkedCount = document.querySelectorAll('.article-checkbox:checked').length;
            
            selectAllCheckbox.checked = checkboxes.length > 0 && checkedCount === checkboxes.length;
            selectAllCheckbox.indeterminate = checkedCount > 0 && checkedCount < checkboxes.length;
        }
    }

    // 切換全選
    toggleSelectAll(checked) {
        const checkboxes = document.querySelectorAll('.article-checkbox');
        this.selectedArticles.clear();
        
        checkboxes.forEach(checkbox => {
            checkbox.checked = checked;
            if (checked) {
                this.selectedArticles.add(parseInt(checkbox.value));
            }
        });
        
        this.updateBatchActions();
    }

    // 處理批量操作
    async handleBatchAction(action) {
        if (this.selectedArticles.size === 0) {
            Utils.showNotification('請先選擇要操作的文章', 'warning');
            return;
        }

        const articleIds = Array.from(this.selectedArticles);
        let actionData = {};
        let confirmMessage = '';

        switch (action) {
            case 'publish':
                actionData = { status: 'published' };
                confirmMessage = `確定要發布選中的 ${articleIds.length} 篇文章嗎？`;
                break;
            case 'draft':
                actionData = { status: 'draft' };
                confirmMessage = `確定要將選中的 ${articleIds.length} 篇文章設為草稿嗎？`;
                break;
            case 'feature':
                actionData = { featured: true };
                confirmMessage = `確定要將選中的 ${articleIds.length} 篇文章設為精選嗎？`;
                break;
            case 'delete':
                confirmMessage = `確定要刪除選中的 ${articleIds.length} 篇文章嗎？此操作無法復原！`;
                break;
        }

        if (!confirm(confirmMessage)) return;

        try {
            const result = await adminAPI.batchUpdateArticles(
                articleIds, 
                action === 'delete' ? 'delete' : (action === 'feature' ? 'updateFeatured' : 'updateStatus'),
                actionData
            );

            if (result.success) {
                Utils.showNotification(result.message, 'success');
                this.selectedArticles.clear();
                await this.loadArticles();
                this.updateBatchActions();
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('批量操作失敗:', error);
            Utils.showNotification('操作失敗，請稍後再試', 'error');
        }
    }

    // 渲染分頁
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
                <button class="page-btn" onclick="adminManager.goToPage(${current - 1})">
                    上一頁
                </button>
            `;
        }

        // 頁碼
        const startPage = Math.max(1, current - 2);
        const endPage = Math.min(total, current + 2);

        for (let i = startPage; i <= endPage; i++) {
            paginationHTML += `
                <button class="page-btn ${i === current ? 'active' : ''}" 
                        onclick="adminManager.goToPage(${i})">
                    ${i}
                </button>
            `;
        }

        // 下一頁
        if (current < total) {
            paginationHTML += `
                <button class="page-btn" onclick="adminManager.goToPage(${current + 1})">
                    下一頁
                </button>
            `;
        }

        paginationHTML += '</div>';
        container.innerHTML = paginationHTML;
    }

    // 跳轉頁面
    goToPage(page) {
        this.currentPage = page;
        this.loadArticles();
    }

    // 顯示編輯視圖
    showEditView(article = null) {
        const listView = document.getElementById('articlesListView');
        const editView = document.getElementById('articleEditView');
        const editPageTitle = document.getElementById('editPageTitle');

        if (listView) listView.classList.add('d-none');
        if (editView) editView.classList.remove('d-none');

        this.isEditing = !!article;
        this.editingArticleId = article ? article.id : null;

        if (editPageTitle) {
            editPageTitle.textContent = article ? '編輯文章' : '新增文章';
        }

        if (article) {
            this.populateEditForm(article);
        } else {
            this.resetEditForm();
        }

        // 更新 URL
        const newURL = article ? 
            `/admin/Article/Edit/${article.id}` : 
            '/admin/Article/Create';
        window.history.pushState({}, '', newURL);
    }

    // 顯示列表視圖
    showListView() {
        const listView = document.getElementById('articlesListView');
        const editView = document.getElementById('articleEditView');

        if (listView) listView.classList.remove('d-none');
        if (editView) editView.classList.add('d-none');

        this.isEditing = false;
        this.editingArticleId = null;

        // 更新 URL
        window.history.pushState({}, '', 'articles.html');
    }

    // 編輯文章
    async editArticle(id) {
        try {
            const result = await adminAPI.getArticle(id);
            if (result.success) {
                this.showEditView(result.data);
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('載入文章失敗:', error);
            Utils.showNotification('載入文章失敗', 'error');
        }
    }

    // 填充編輯表單
    populateEditForm(article) {
        document.getElementById('articleId').value = article.id;
        document.getElementById('articleTitle').value = article.title;
        document.getElementById('articleExcerpt').value = article.excerpt || '';
        document.getElementById('articleAuthor').value = article.author;
        document.getElementById('articleCategory').value = article.category;
        document.getElementById('articleTags').value = article.tags.join(', ');
        document.getElementById('articleReadTime').value = article.readTime;
        document.getElementById('articleImage').value = article.image || '';
        document.getElementById('articleStatus').value = article.status;
        document.getElementById('articleFeatured').checked = article.featured;
        document.getElementById('articleContent').value = article.content;
    }

    // 重置編輯表單
    resetEditForm() {
        document.getElementById('articleForm').reset();
        document.getElementById('articleId').value = '';
        document.getElementById('articleAuthor').value = '管理員';
        document.getElementById('articleReadTime').value = '5';
        document.getElementById('articleStatus').value = 'draft';
        document.getElementById('articleFeatured').checked = false;
    }

    // 儲存文章
    async saveArticle() {
        const form = document.getElementById('articleForm');
        const formData = new FormData(form);
        
        // 收集表單資料
        const articleData = {
            title: formData.get('title').trim(),
            excerpt: formData.get('excerpt').trim(),
            author: formData.get('author').trim(),
            category: formData.get('category'),
            tags: formData.get('tags').split(',').map(tag => tag.trim()).filter(tag => tag),
            readTime: parseInt(formData.get('readTime')),
            image: formData.get('image').trim(),
            status: formData.get('status'),
            featured: formData.has('featured'),
            content: formData.get('content').trim(),
            publishDate: new Date().toISOString()
        };

        // 驗證必填欄位
        if (!articleData.title) {
            Utils.showNotification('請輸入文章標題', 'warning');
            return;
        }

        if (!articleData.category) {
            Utils.showNotification('請選擇文章分類', 'warning');
            return;
        }

        if (!articleData.content) {
            Utils.showNotification('請輸入文章內容', 'warning');
            return;
        }

        try {
            let result;
            if (this.isEditing) {
                result = await adminAPI.updateArticle(this.editingArticleId, articleData);
            } else {
                result = await adminAPI.createArticle(articleData);
            }

            if (result.success) {
                Utils.showNotification(result.message, 'success');
                this.showListView();
                await this.loadArticles();
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('儲存文章失敗:', error);
            Utils.showNotification('儲存失敗，請稍後再試', 'error');
        }
    }

    // 刪除文章
    async deleteArticle(id) {
        if (!confirm('確定要刪除這篇文章嗎？此操作無法復原！')) {
            return;
        }

        try {
            const result = await adminAPI.deleteArticle(id);
            if (result.success) {
                Utils.showNotification(result.message, 'success');
                await this.loadArticles();
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('刪除文章失敗:', error);
            Utils.showNotification('刪除失敗，請稍後再試', 'error');
        }
    }

    // 處理編輯器操作
    handleEditorAction(action) {
        const textarea = document.getElementById('articleContent');
        if (!textarea) return;

        const start = textarea.selectionStart;
        const end = textarea.selectionEnd;
        const selectedText = textarea.value.substring(start, end);
        let replacement = '';

        switch (action) {
            case 'bold':
                replacement = `**${selectedText || '粗體文字'}**`;
                break;
            case 'italic':
                replacement = `*${selectedText || '斜體文字'}*`;
                break;
            case 'heading':
                replacement = `## ${selectedText || '標題'}`;
                break;
            case 'code':
                replacement = selectedText.includes('\n') ? 
                    `\`\`\`\n${selectedText || '程式碼'}\n\`\`\`` : 
                    `\`${selectedText || '程式碼'}\``;
                break;
            case 'link':
                replacement = `[${selectedText || '連結文字'}](網址)`;
                break;
        }

        textarea.value = textarea.value.substring(0, start) + replacement + textarea.value.substring(end);
        textarea.focus();
        
        // 設定游標位置
        const newPos = start + replacement.length;
        textarea.setSelectionRange(newPos, newPos);
    }

    // 綁定全域事件
    bindGlobalEvents() {
        // 快速操作按鈕
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('quick-action-btn')) {
                e.preventDefault();
                const href = e.target.getAttribute('href');
                if (href && !href.startsWith('#') && !href.includes('://')) {
                    window.location.href = href;
                }
            }
        });
    }

    // 獲取狀態文字
    getStatusText(status) {
        const statusMap = {
            'published': '已發布',
            'draft': '草稿'
        };
        return statusMap[status] || status;
    }
}

// 頁面載入完成後初始化
document.addEventListener('DOMContentLoaded', () => {
    window.adminManager = new AdminManager();
}); 