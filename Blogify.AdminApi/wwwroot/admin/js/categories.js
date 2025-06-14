// 分類管理邏輯
class CategoryManager {
    constructor() {
        this.currentFilters = {
            search: '',
            sort: 'name'
        };
        this.isEditing = false;
        this.editingCategoryId = null;
        this.deleteTargetId = null;
        
        this.init();
    }

    async init() {
        // 檢查認證
        if (!Auth.isAuthenticated()) {
            window.location.href = 'login.html';
            return;
        }

        try {
            await this.loadCategories();
            await this.loadCategoryStats();
            this.bindEvents();
        } catch (error) {
            console.error('初始化分類管理失敗:', error);
            Utils.showNotification('載入分類管理頁面失敗', 'error');
        }
    }

    // 綁定事件
    bindEvents() {
        // 新增分類按鈕
        const newCategoryBtn = document.getElementById('newCategoryBtn');
        const quickNewCategoryBtn = document.getElementById('quickNewCategoryBtn');
        
        if (newCategoryBtn) {
            newCategoryBtn.addEventListener('click', () => this.showEditView());
        }
        if (quickNewCategoryBtn) {
            quickNewCategoryBtn.addEventListener('click', () => this.showEditView());
        }

        // 取消編輯按鈕
        const cancelEditBtn = document.getElementById('cancelEditBtn');
        if (cancelEditBtn) {
            cancelEditBtn.addEventListener('click', () => this.showListView());
        }

        // 分類表單提交
        const categoryForm = document.getElementById('categoryForm');
        if (categoryForm) {
            categoryForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.saveCategory();
            });
        }

        // 搜尋和排序
        this.bindFilterEvents();

        // 顏色選擇器
        this.bindColorPickerEvents();

        // 模態框事件
        this.bindModalEvents();

        // 匯出/匯入按鈕
        this.bindImportExportEvents();

        // 分類名稱自動生成代碼
        this.bindSlugGeneration();

        // 登出按鈕
        const logoutBtn = document.querySelector('[data-action="logout"]');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', () => {
                Auth.logout();
                window.location.href = 'login.html';
            });
        }
    }

    // 綁定篩選器事件
    bindFilterEvents() {
        const searchInput = document.getElementById('searchInput');
        const sortFilter = document.getElementById('sortFilter');

        if (searchInput) {
            const debouncedSearch = Utils.debounce(() => {
                this.currentFilters.search = searchInput.value.trim();
                this.loadCategories();
            }, 500);
            searchInput.addEventListener('input', debouncedSearch);
        }

        if (sortFilter) {
            sortFilter.addEventListener('change', () => {
                this.currentFilters.sort = sortFilter.value;
                this.loadCategories();
            });
        }
    }

    // 綁定顏色選擇器事件
    bindColorPickerEvents() {
        const colorPresets = document.querySelectorAll('.color-preset');
        const colorPicker = document.getElementById('categoryColor');

        colorPresets.forEach(preset => {
            preset.addEventListener('click', (e) => {
                e.preventDefault();
                const color = e.target.dataset.color;
                if (colorPicker) {
                    colorPicker.value = color;
                }
                
                // 更新選中狀態
                colorPresets.forEach(p => p.classList.remove('active'));
                e.target.classList.add('active');
            });
        });
    }

    // 綁定模態框事件
    bindModalEvents() {
        const modal = document.getElementById('deleteModal');
        const closeButtons = document.querySelectorAll('[data-action="close-modal"]');
        const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');

        closeButtons.forEach(btn => {
            btn.addEventListener('click', () => this.hideModal());
        });

        if (confirmDeleteBtn) {
            confirmDeleteBtn.addEventListener('click', () => this.confirmDelete());
        }

        // 點擊模態框背景關閉
        if (modal) {
            modal.addEventListener('click', (e) => {
                if (e.target === modal) {
                    this.hideModal();
                }
            });
        }
    }

    // 綁定匯出/匯入事件
    bindImportExportEvents() {
        const exportBtn = document.getElementById('exportCategoriesBtn');
        const importBtn = document.getElementById('importCategoriesBtn');

        if (exportBtn) {
            exportBtn.addEventListener('click', () => this.exportCategories());
        }

        if (importBtn) {
            importBtn.addEventListener('click', () => this.importCategories());
        }
    }

    // 綁定代碼自動生成
    bindSlugGeneration() {
        const nameInput = document.getElementById('categoryName');
        const slugInput = document.getElementById('categorySlug');

        if (nameInput && slugInput) {
            nameInput.addEventListener('input', () => {
                if (!slugInput.value || slugInput.dataset.autoGenerated === 'true') {
                    const slug = this.generateSlug(nameInput.value);
                    slugInput.value = slug;
                    slugInput.dataset.autoGenerated = 'true';
                }
            });

            slugInput.addEventListener('input', () => {
                slugInput.dataset.autoGenerated = 'false';
            });
        }
    }

    // 生成代碼
    generateSlug(name) {
        return name
            .toLowerCase()
            .replace(/[^\w\s-]/g, '') // 移除特殊字符
            .replace(/[\s_-]+/g, '-') // 替換空格和下劃線為連字符
            .replace(/^-+|-+$/g, ''); // 移除開頭和結尾的連字符
    }

    // 載入分類列表
    async loadCategories() {
        const tableBody = document.getElementById('categoriesTableBody');
        if (!tableBody) return;

        // 顯示載入狀態
        tableBody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center p-4">
                    <div class="spinner"></div>
                    <p class="mt-3">載入中...</p>
                </td>
            </tr>
        `;

        try {
            const result = await adminAPI.getCategories(this.currentFilters);

            if (result.success) {
                this.renderCategoriesTable(result.data);
            } else {
                throw new Error(result.error);
            }
        } catch (error) {
            console.error('載入分類失敗:', error);
            tableBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center p-4">
                        <p class="text-danger">載入失敗：${error.message}</p>
                        <button class="btn btn-primary btn-sm mt-2" onclick="categoryManager.loadCategories()">重試</button>
                    </td>
                </tr>
            `;
        }
    }

    // 渲染分類表格
    renderCategoriesTable(categories) {
        const tableBody = document.getElementById('categoriesTableBody');
        if (!tableBody) return;

        if (categories.length === 0) {
            tableBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center p-4">
                        <p class="text-muted">沒有找到符合條件的分類</p>
                        <button class="btn btn-primary btn-sm mt-2" onclick="categoryManager.showEditView()">新增第一個分類</button>
                    </td>
                </tr>
            `;
            return;
        }

        tableBody.innerHTML = categories.map(category => `
            <tr>
                <td>
                    <div class="category-name-cell">
                        <span class="category-icon" style="color: ${category.color}">${category.icon || '📁'}</span>
                        <strong>${category.name}</strong>
                        ${!category.visible ? '<span class="status-badge status-draft">隱藏</span>' : ''}
                    </div>
                </td>
                <td>
                    <div class="category-description">
                        ${category.description || '<span class="text-muted">無描述</span>'}
                    </div>
                </td>
                <td>
                    <span class="article-count">${category.count}</span>
                </td>
                <td>
                    <div class="color-display">
                        <div class="color-swatch" style="background: ${category.color}"></div>
                        <span class="color-code">${category.color}</span>
                    </div>
                </td>
                <td>${Utils.formatDate(category.createdAt)}</td>
                <td>
                    <div class="table-actions">
                        <button class="action-btn btn-edit" onclick="categoryManager.editCategory(${category.id})" title="編輯">
                            <i class="icon-edit"></i>
                        </button>
                        <button class="action-btn btn-view" onclick="window.open('../client/index.html?category=${encodeURIComponent(category.name)}', '_blank')" title="查看">
                            <i class="icon-view"></i>
                        </button>
                        <button class="action-btn btn-delete" onclick="categoryManager.showDeleteModal(${category.id}, '${category.name}', ${category.count})" title="刪除">
                            <i class="icon-delete"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    }

    // 載入分類統計
    async loadCategoryStats() {
        try {
            const result = await adminAPI.getCategoryStats();
            if (result.success) {
                this.renderCategoryStats(result.data);
                this.renderCategoryStatsDetail(result.data);
            }
        } catch (error) {
            console.error('載入分類統計失敗:', error);
        }
    }

    // 渲染分類統計
    renderCategoryStats(stats) {
        const container = document.getElementById('categoryStatsContainer');
        if (!container) return;

        container.innerHTML = `
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">總分類數</span>
                    <div class="stat-icon">📁</div>
                </div>
                <div class="stat-value">${stats.totalCategories}</div>
                <div class="stat-change">${stats.visibleCategories} 個顯示中</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">最多文章</span>
                    <div class="stat-icon">📝</div>
                </div>
                <div class="stat-value">${stats.maxArticlesInCategory}</div>
                <div class="stat-change">${stats.mostPopularCategory}</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">平均文章數</span>
                    <div class="stat-icon">📊</div>
                </div>
                <div class="stat-value">${stats.averageArticlesPerCategory.toFixed(1)}</div>
                <div class="stat-change">每個分類</div>
            </div>
            <div class="stat-card">
                <div class="stat-header">
                    <span class="stat-title">空分類</span>
                    <div class="stat-icon">📄</div>
                </div>
                <div class="stat-value">${stats.emptyCategoriesCount}</div>
                <div class="stat-change">待添加內容</div>
            </div>
        `;
    }

    // 渲染分類統計詳情
    renderCategoryStatsDetail(stats) {
        const container = document.getElementById('categoryStatsDetail');
        if (!container) return;

        container.innerHTML = `
            <div class="stats-detail-list">
                ${stats.categoryDetails.map(category => `
                    <div class="stats-detail-item">
                        <div class="category-info">
                            <span class="category-icon" style="color: ${category.color}">${category.icon || '📁'}</span>
                            <span class="category-name">${category.name}</span>
                        </div>
                        <div class="category-count">
                            <span class="count-number">${category.count}</span>
                            <span class="count-label">篇</span>
                        </div>
                    </div>
                `).join('')}
            </div>
        `;
    }

    // 顯示編輯視圖
    showEditView(category = null) {
        const listView = document.getElementById('categoriesListView');
        const editView = document.getElementById('categoryEditView');
        const editPageTitle = document.getElementById('editPageTitle');

        if (listView) listView.classList.add('d-none');
        if (editView) editView.classList.remove('d-none');

        this.isEditing = !!category;
        this.editingCategoryId = category ? category.id : null;

        if (editPageTitle) {
            editPageTitle.textContent = category ? '編輯分類' : '新增分類';
        }

        if (category) {
            this.populateEditForm(category);
        } else {
            this.resetEditForm();
        }
    }

    // 顯示列表視圖
    showListView() {
        const listView = document.getElementById('categoriesListView');
        const editView = document.getElementById('categoryEditView');

        if (listView) listView.classList.remove('d-none');
        if (editView) editView.classList.add('d-none');

        this.isEditing = false;
        this.editingCategoryId = null;
    }

    // 編輯分類
    async editCategory(id) {
        try {
            const result = await adminAPI.getCategory(id);
            if (result.success) {
                this.showEditView(result.data);
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('載入分類失敗:', error);
            Utils.showNotification('載入分類失敗', 'error');
        }
    }

    // 填充編輯表單
    populateEditForm(category) {
        document.getElementById('categoryId').value = category.id;
        document.getElementById('categoryName').value = category.name;
        document.getElementById('categorySlug').value = category.slug || '';
        document.getElementById('categoryDescription').value = category.description || '';
        document.getElementById('categoryColor').value = category.color;
        document.getElementById('categoryIcon').value = category.icon || '';
        document.getElementById('categoryOrder').value = category.order || 0;
        document.getElementById('categoryVisible').checked = category.visible !== false;

        // 更新顏色預設選擇
        const colorPresets = document.querySelectorAll('.color-preset');
        colorPresets.forEach(preset => {
            preset.classList.toggle('active', preset.dataset.color === category.color);
        });
    }

    // 重置編輯表單
    resetEditForm() {
        document.getElementById('categoryForm').reset();
        document.getElementById('categoryId').value = '';
        document.getElementById('categoryColor').value = '#007bff';
        document.getElementById('categoryOrder').value = '0';
        document.getElementById('categoryVisible').checked = true;

        // 重置顏色預設選擇
        const colorPresets = document.querySelectorAll('.color-preset');
        colorPresets.forEach(preset => {
            preset.classList.toggle('active', preset.dataset.color === '#007bff');
        });
    }

    // 儲存分類
    async saveCategory() {
        const form = document.getElementById('categoryForm');
        const formData = new FormData(form);
        
        // 收集表單資料
        const categoryData = {
            name: formData.get('name').trim(),
            slug: formData.get('slug').trim(),
            description: formData.get('description').trim(),
            color: formData.get('color'),
            icon: formData.get('icon').trim(),
            order: parseInt(formData.get('order')) || 0,
            visible: formData.has('visible')
        };

        // 驗證必填欄位
        if (!categoryData.name) {
            Utils.showNotification('請輸入分類名稱', 'warning');
            return;
        }

        // 自動生成代碼
        if (!categoryData.slug) {
            categoryData.slug = this.generateSlug(categoryData.name);
        }

        try {
            let result;
            if (this.isEditing) {
                result = await adminAPI.updateCategory(this.editingCategoryId, categoryData);
            } else {
                result = await adminAPI.createCategory(categoryData);
            }

            if (result.success) {
                Utils.showNotification(result.message, 'success');
                this.showListView();
                await this.loadCategories();
                await this.loadCategoryStats();
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('儲存分類失敗:', error);
            Utils.showNotification('儲存失敗，請稍後再試', 'error');
        }
    }

    // 顯示刪除模態框
    showDeleteModal(id, name, count) {
        const modal = document.getElementById('deleteModal');
        const nameSpan = document.getElementById('deleteCategoryName');
        const countSpan = document.getElementById('deleteCategoryCount');

        if (nameSpan) nameSpan.textContent = name;
        if (countSpan) countSpan.textContent = count;

        this.deleteTargetId = id;
        
        if (modal) {
            modal.classList.remove('d-none');
            document.body.style.overflow = 'hidden';
        }
    }

    // 隱藏模態框
    hideModal() {
        const modal = document.getElementById('deleteModal');
        if (modal) {
            modal.classList.add('d-none');
            document.body.style.overflow = '';
        }
        this.deleteTargetId = null;
    }

    // 確認刪除
    async confirmDelete() {
        if (!this.deleteTargetId) return;

        try {
            const result = await adminAPI.deleteCategory(this.deleteTargetId);
            if (result.success) {
                Utils.showNotification(result.message, 'success');
                this.hideModal();
                await this.loadCategories();
                await this.loadCategoryStats();
            } else {
                Utils.showNotification(result.error, 'error');
            }
        } catch (error) {
            console.error('刪除分類失敗:', error);
            Utils.showNotification('刪除失敗，請稍後再試', 'error');
        }
    }

    // 匯出分類
    async exportCategories() {
        try {
            const result = await adminAPI.getCategories({ includeHidden: true });
            if (result.success) {
                const dataStr = JSON.stringify(result.data, null, 2);
                const dataBlob = new Blob([dataStr], { type: 'application/json' });
                
                const link = document.createElement('a');
                link.href = URL.createObjectURL(dataBlob);
                link.download = `categories_${new Date().toISOString().split('T')[0]}.json`;
                link.click();
                
                Utils.showNotification('分類資料已匯出', 'success');
            }
        } catch (error) {
            console.error('匯出失敗:', error);
            Utils.showNotification('匯出失敗', 'error');
        }
    }

    // 匯入分類
    importCategories() {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json';
        
        input.onchange = async (e) => {
            const file = e.target.files[0];
            if (!file) return;
            
            try {
                const text = await file.text();
                const categories = JSON.parse(text);
                
                if (!Array.isArray(categories)) {
                    throw new Error('檔案格式不正確');
                }
                
                const result = await adminAPI.importCategories(categories);
                if (result.success) {
                    Utils.showNotification(result.message, 'success');
                    await this.loadCategories();
                    await this.loadCategoryStats();
                } else {
                    Utils.showNotification(result.error, 'error');
                }
            } catch (error) {
                console.error('匯入失敗:', error);
                Utils.showNotification('匯入失敗：檔案格式不正確', 'error');
            }
        };
        
        input.click();
    }
}

// 頁面載入完成後初始化
document.addEventListener('DOMContentLoaded', () => {
    window.categoryManager = new CategoryManager();
}); 