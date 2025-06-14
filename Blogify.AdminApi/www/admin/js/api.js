// 管理端 API 模擬
class AdminAPI {
    constructor() {
        this.baseURL = '/api/admin';
        this.initMockData();
    }

    // 初始化假資料
    initMockData() {
        // 管理員帳號
        this.mockAdmin = {
            username: 'admin',
            password: 'password',
            name: '管理員',
            email: 'admin@myblog.com'
        };

        // 文章資料（與客戶端共用，但包含更多管理資訊）
        this.mockArticles = [
            {
                id: 1,
                title: "JavaScript ES6+ 新特性完整指南",
                content: `JavaScript ES6+ 帶來了許多強大的新特性，讓開發者能夠寫出更簡潔、更高效的程式碼。

## 箭頭函數
箭頭函數提供了更簡潔的函數語法：
\`\`\`javascript
const add = (a, b) => a + b;
\`\`\`

## 解構賦值
解構賦值讓我們能夠從陣列或物件中提取值：
\`\`\`javascript
const [first, second] = [1, 2];
const {name, age} = person;
\`\`\`

## 模板字串
模板字串讓字串插值變得更容易：
\`\`\`javascript
const message = \`Hello, \${name}!\`;
\`\`\`

這些特性大大提升了 JavaScript 的開發體驗。`,
                excerpt: "深入了解 JavaScript ES6+ 的新特性，包含箭頭函數、解構賦值、模板字串等實用功能。",
                author: "技術小編",
                category: "前端開發",
                tags: ["JavaScript", "ES6", "前端"],
                publishDate: "2024-01-15T10:30:00Z",
                readTime: 8,
                views: 1250,
                likes: 89,
                status: "published",
                featured: true,
                image: "https://via.placeholder.com/800x400/007bff/ffffff?text=JavaScript+ES6%2B",
                createdAt: "2024-01-15T10:30:00Z",
                updatedAt: "2024-01-15T10:30:00Z"
            },
            {
                id: 2,
                title: "CSS Grid 與 Flexbox 完整比較",
                content: `CSS Grid 和 Flexbox 都是現代 CSS 佈局的重要工具，但它們各有適用的場景。

## CSS Grid
CSS Grid 是二維佈局系統，適合複雜的網格佈局：
\`\`\`css
.grid-container {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
}
\`\`\`

## Flexbox
Flexbox 是一維佈局系統，適合元件內部的佈局：
\`\`\`css
.flex-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
}
\`\`\`

## 何時使用哪個？
- 使用 Grid：頁面整體佈局、複雜的二維佈局
- 使用 Flexbox：元件內部佈局、一維對齊

兩者結合使用能創造出強大且靈活的佈局系統。`,
                excerpt: "詳細比較 CSS Grid 和 Flexbox 的特點與使用場景，幫助你選擇最適合的佈局方案。",
                author: "設計師小王",
                category: "前端開發",
                tags: ["CSS", "Grid", "Flexbox", "佈局"],
                publishDate: "2024-01-12T14:20:00Z",
                readTime: 6,
                views: 980,
                likes: 67,
                status: "published",
                featured: false,
                image: "https://via.placeholder.com/800x400/28a745/ffffff?text=CSS+Grid+vs+Flexbox",
                createdAt: "2024-01-12T14:20:00Z",
                updatedAt: "2024-01-12T14:20:00Z"
            },
            {
                id: 3,
                title: "Node.js 效能優化實戰技巧",
                content: `Node.js 應用的效能優化是後端開發的重要課題。以下是一些實用的優化技巧。

## 1. 使用叢集模式
利用多核心 CPU 的優勢：
\`\`\`javascript
const cluster = require('cluster');
const numCPUs = require('os').cpus().length;

if (cluster.isMaster) {
    for (let i = 0; i < numCPUs; i++) {
        cluster.fork();
    }
} else {
    // 啟動應用
    app.listen(3000);
}
\`\`\`

## 2. 記憶體管理
避免記憶體洩漏：
- 及時清理事件監聽器
- 避免全域變數累積
- 使用 WeakMap 和 WeakSet

## 3. 資料庫優化
- 建立適當的索引
- 使用連線池
- 實作查詢快取

## 4. 快取策略
- Redis 快取熱點資料
- HTTP 快取標頭
- CDN 靜態資源

這些技巧能顯著提升 Node.js 應用的效能。`,
                excerpt: "分享 Node.js 效能優化的實戰經驗，包含叢集、記憶體管理、資料庫優化等技巧。",
                author: "後端工程師",
                category: "後端開發",
                tags: ["Node.js", "效能優化", "後端"],
                publishDate: "2024-01-10T09:15:00Z",
                readTime: 10,
                views: 1580,
                likes: 124,
                status: "published",
                featured: true,
                image: "https://via.placeholder.com/800x400/ffc107/000000?text=Node.js+Performance",
                createdAt: "2024-01-10T09:15:00Z",
                updatedAt: "2024-01-10T09:15:00Z"
            },
            {
                id: 4,
                title: "React Hooks 深度解析",
                content: `React Hooks 徹底改變了 React 開發的方式，讓函數元件也能擁有狀態和生命週期。

## useState Hook
管理元件狀態：
\`\`\`javascript
const [count, setCount] = useState(0);
\`\`\`

## useEffect Hook
處理副作用：
\`\`\`javascript
useEffect(() => {
    document.title = \`Count: \${count}\`;
}, [count]);
\`\`\`

## 自定義 Hook
封裝可重用的邏輯：
\`\`\`javascript
function useCounter(initialValue = 0) {
    const [count, setCount] = useState(initialValue);
    
    const increment = () => setCount(count + 1);
    const decrement = () => setCount(count - 1);
    
    return { count, increment, decrement };
}
\`\`\`

## 最佳實踐
- 遵循 Hook 規則
- 合理拆分 useEffect
- 使用 useMemo 和 useCallback 優化效能

Hooks 讓 React 開發更加簡潔和強大。`,
                excerpt: "深入探討 React Hooks 的使用方法和最佳實踐，包含 useState、useEffect 和自定義 Hook。",
                author: "React 專家",
                category: "前端開發",
                tags: ["React", "Hooks", "前端"],
                publishDate: "2024-01-08T16:45:00Z",
                readTime: 12,
                views: 2100,
                likes: 156,
                status: "draft",
                featured: false,
                image: "https://via.placeholder.com/800x400/61dafb/000000?text=React+Hooks",
                createdAt: "2024-01-08T16:45:00Z",
                updatedAt: "2024-01-08T16:45:00Z"
            },
            {
                id: 5,
                title: "MongoDB 資料庫設計最佳實踐",
                content: `MongoDB 作為 NoSQL 資料庫，有其獨特的設計原則和最佳實踐。

## 文件結構設計
合理的文件結構是關鍵：
\`\`\`javascript
{
    _id: ObjectId("..."),
    title: "文章標題",
    content: "文章內容",
    author: {
        id: ObjectId("..."),
        name: "作者姓名"
    },
    tags: ["tag1", "tag2"],
    createdAt: ISODate("...")
}
\`\`\`

## 索引策略
建立適當的索引提升查詢效能：
\`\`\`javascript
// 複合索引
db.articles.createIndex({ "author.id": 1, "createdAt": -1 });

// 文字索引
db.articles.createIndex({ title: "text", content: "text" });
\`\`\`

## 嵌入 vs 引用
- 嵌入：一對一、一對少量
- 引用：一對多、多對多

## 效能優化
- 使用投影減少資料傳輸
- 合理使用聚合管道
- 監控慢查詢

正確的設計能讓 MongoDB 發揮最大效能。`,
                excerpt: "分享 MongoDB 資料庫設計的最佳實踐，包含文件結構、索引策略和效能優化。",
                author: "資料庫專家",
                category: "資料庫",
                tags: ["MongoDB", "NoSQL", "資料庫設計"],
                publishDate: "2024-01-05T11:30:00Z",
                readTime: 9,
                views: 890,
                likes: 73,
                status: "published",
                featured: false,
                image: "https://via.placeholder.com/800x400/4db33d/ffffff?text=MongoDB+Best+Practices",
                createdAt: "2024-01-05T11:30:00Z",
                updatedAt: "2024-01-05T11:30:00Z"
            }
        ];

        this.mockCategories = [
            { 
                id: 1, 
                name: "前端開發", 
                slug: "frontend", 
                description: "HTML、CSS、JavaScript 等前端技術", 
                color: "#007bff", 
                icon: "💻", 
                order: 1, 
                visible: true,
                count: 3,
                createdAt: "2024-01-01T00:00:00Z",
                updatedAt: "2024-01-01T00:00:00Z"
            },
            { 
                id: 2, 
                name: "後端開發", 
                slug: "backend", 
                description: "Node.js、Python、Java 等後端技術", 
                color: "#28a745", 
                icon: "⚙️", 
                order: 2, 
                visible: true,
                count: 1,
                createdAt: "2024-01-01T00:00:00Z",
                updatedAt: "2024-01-01T00:00:00Z"
            },
            { 
                id: 3, 
                name: "資料庫", 
                slug: "database", 
                description: "MySQL、MongoDB、Redis 等資料庫技術", 
                color: "#ffc107", 
                icon: "🗄️", 
                order: 3, 
                visible: true,
                count: 1,
                createdAt: "2024-01-01T00:00:00Z",
                updatedAt: "2024-01-01T00:00:00Z"
            },
            { 
                id: 4, 
                name: "DevOps", 
                slug: "devops", 
                description: "部署、監控、CI/CD 等運維技術", 
                color: "#dc3545", 
                icon: "🚀", 
                order: 4, 
                visible: true,
                count: 0,
                createdAt: "2024-01-01T00:00:00Z",
                updatedAt: "2024-01-01T00:00:00Z"
            },
            { 
                id: 5, 
                name: "行動開發", 
                slug: "mobile", 
                description: "iOS、Android、React Native 等行動開發", 
                color: "#17a2b8", 
                icon: "📱", 
                order: 5, 
                visible: true,
                count: 0,
                createdAt: "2024-01-01T00:00:00Z",
                updatedAt: "2024-01-01T00:00:00Z"
            }
        ];

        // 統計資料
        this.mockStats = {
            totalArticles: this.mockArticles.length,
            publishedArticles: this.mockArticles.filter(a => a.status === 'published').length,
            draftArticles: this.mockArticles.filter(a => a.status === 'draft').length,
            totalViews: this.mockArticles.reduce((sum, a) => sum + a.views, 0),
            totalLikes: this.mockArticles.reduce((sum, a) => sum + a.likes, 0),
            featuredArticles: this.mockArticles.filter(a => a.featured).length
        };
    }

    // 模擬 API 延遲
    async delay(ms = 300) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    // 檢查認證狀態
    isAuthenticated() {
        const token = Utils.getStorage('adminToken');
        return !!token;
    }

    // 登入
    async login(username, password) {
        await this.delay();
        
        if (username === this.mockAdmin.username && password === this.mockAdmin.password) {
            const token = 'mock-admin-token-' + Date.now();
            Utils.setStorage('adminToken', token);
            Utils.setStorage('adminUser', this.mockAdmin);
            
            return {
                success: true,
                data: {
                    token,
                    user: this.mockAdmin
                }
            };
        }
        
        return {
            success: false,
            error: '帳號或密碼錯誤'
        };
    }

    // 登出
    async logout() {
        await this.delay(100);
        
        Utils.removeStorage('adminToken');
        Utils.removeStorage('adminUser');
        
        return {
            success: true,
            message: '已成功登出'
        };
    }

    // 獲取當前使用者
    getCurrentUser() {
        return Utils.getStorage('adminUser');
    }

    // 獲取統計資料
    async getStats() {
        await this.delay();
        
        // 重新計算統計資料
        this.mockStats = {
            totalArticles: this.mockArticles.length,
            publishedArticles: this.mockArticles.filter(a => a.status === 'published').length,
            draftArticles: this.mockArticles.filter(a => a.status === 'draft').length,
            totalViews: this.mockArticles.reduce((sum, a) => sum + a.views, 0),
            totalLikes: this.mockArticles.reduce((sum, a) => sum + a.likes, 0),
            featuredArticles: this.mockArticles.filter(a => a.featured).length
        };
        
        return {
            success: true,
            data: this.mockStats
        };
    }

    // 獲取文章列表（管理端）
    async getArticles(params = {}) {
        await this.delay();
        
        let articles = [...this.mockArticles];
        
        // 狀態篩選
        if (params.status) {
            articles = articles.filter(article => article.status === params.status);
        }
        
        // 分類篩選
        if (params.category) {
            articles = articles.filter(article => article.category === params.category);
        }
        
        // 搜尋
        if (params.search) {
            const searchTerm = params.search.toLowerCase();
            articles = articles.filter(article =>
                article.title.toLowerCase().includes(searchTerm) ||
                article.content.toLowerCase().includes(searchTerm) ||
                article.author.toLowerCase().includes(searchTerm)
            );
        }
        
        // 排序
        if (params.sort === 'title') {
            articles.sort((a, b) => a.title.localeCompare(b.title));
        } else if (params.sort === 'author') {
            articles.sort((a, b) => a.author.localeCompare(b.author));
        } else if (params.sort === 'views') {
            articles.sort((a, b) => b.views - a.views);
        } else {
            // 預設按更新時間排序
            articles.sort((a, b) => new Date(b.updatedAt) - new Date(a.updatedAt));
        }
        
        // 分頁
        const page = params.page || 1;
        const limit = params.limit || 10;
        const startIndex = (page - 1) * limit;
        const endIndex = startIndex + limit;
        
        const paginatedArticles = articles.slice(startIndex, endIndex);
        
        return {
            success: true,
            data: {
                articles: paginatedArticles,
                pagination: {
                    current: page,
                    total: Math.ceil(articles.length / limit),
                    count: articles.length,
                    limit: limit
                }
            }
        };
    }

    // 獲取單篇文章（管理端）
    async getArticle(id) {
        await this.delay();
        
        const article = this.mockArticles.find(a => a.id == id);
        
        if (!article) {
            return {
                success: false,
                error: '文章不存在'
            };
        }
        
        return {
            success: true,
            data: article
        };
    }

    // 建立文章
    async createArticle(articleData) {
        await this.delay();
        
        const newArticle = {
            id: Math.max(...this.mockArticles.map(a => a.id)) + 1,
            ...articleData,
            views: 0,
            likes: 0,
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
        };
        
        this.mockArticles.unshift(newArticle);
        
        return {
            success: true,
            data: newArticle,
            message: '文章建立成功'
        };
    }

    // 更新文章
    async updateArticle(id, articleData) {
        await this.delay();
        
        const index = this.mockArticles.findIndex(a => a.id == id);
        
        if (index === -1) {
            return {
                success: false,
                error: '文章不存在'
            };
        }
        
        this.mockArticles[index] = {
            ...this.mockArticles[index],
            ...articleData,
            updatedAt: new Date().toISOString()
        };
        
        return {
            success: true,
            data: this.mockArticles[index],
            message: '文章更新成功'
        };
    }

    // 刪除文章
    async deleteArticle(id) {
        await this.delay();
        
        const index = this.mockArticles.findIndex(a => a.id == id);
        
        if (index === -1) {
            return {
                success: false,
                error: '文章不存在'
            };
        }
        
        this.mockArticles.splice(index, 1);
        
        return {
            success: true,
            message: '文章刪除成功'
        };
    }

    // 批量操作文章
    async batchUpdateArticles(ids, action, data = {}) {
        await this.delay();
        
        let updatedCount = 0;
        
        ids.forEach(id => {
            const index = this.mockArticles.findIndex(a => a.id == id);
            if (index !== -1) {
                if (action === 'delete') {
                    this.mockArticles.splice(index, 1);
                } else if (action === 'updateStatus') {
                    this.mockArticles[index].status = data.status;
                    this.mockArticles[index].updatedAt = new Date().toISOString();
                } else if (action === 'updateFeatured') {
                    this.mockArticles[index].featured = data.featured;
                    this.mockArticles[index].updatedAt = new Date().toISOString();
                }
                updatedCount++;
            }
        });
        
        return {
            success: true,
            message: `成功處理 ${updatedCount} 篇文章`,
            data: { updatedCount }
        };
    }

    // 獲取分類列表（管理端）
    async getCategories(params = {}) {
        await this.delay();
        
        // 重新計算分類文章數量
        this.mockCategories.forEach(category => {
            category.count = this.mockArticles.filter(
                article => article.category === category.name
            ).length;
        });
        
        let categories = [...this.mockCategories];
        
        // 搜尋篩選
        if (params.search) {
            const searchTerm = params.search.toLowerCase();
            categories = categories.filter(category => 
                category.name.toLowerCase().includes(searchTerm) ||
                (category.description && category.description.toLowerCase().includes(searchTerm))
            );
        }
        
        // 排序
        if (params.sort) {
            categories.sort((a, b) => {
                switch (params.sort) {
                    case 'name':
                        return a.name.localeCompare(b.name);
                    case 'count':
                        return b.count - a.count;
                    case 'createdAt':
                        return new Date(b.createdAt) - new Date(a.createdAt);
                    default:
                        return 0;
                }
            });
        }
        
        // 是否包含隱藏分類
        if (!params.includeHidden) {
            categories = categories.filter(category => category.visible !== false);
        }
        
        return {
            success: true,
            data: categories
        };
    }

    // 獲取單個分類
    async getCategory(id) {
        await this.delay();
        
        const category = this.mockCategories.find(c => c.id == id);
        
        if (!category) {
            return {
                success: false,
                error: '分類不存在'
            };
        }
        
        // 計算文章數量
        const count = this.mockArticles.filter(
            article => article.category === category.name
        ).length;
        
        return {
            success: true,
            data: { ...category, count }
        };
    }

    // 獲取分類統計
    async getCategoryStats() {
        await this.delay();
        
        // 重新計算分類文章數量
        this.mockCategories.forEach(category => {
            category.count = this.mockArticles.filter(
                article => article.category === category.name
            ).length;
        });
        
        const totalCategories = this.mockCategories.length;
        const visibleCategories = this.mockCategories.filter(c => c.visible !== false).length;
        const emptyCategoriesCount = this.mockCategories.filter(c => c.count === 0).length;
        
        const categoryCounts = this.mockCategories.map(c => c.count);
        const maxArticlesInCategory = Math.max(...categoryCounts, 0);
        const averageArticlesPerCategory = totalCategories > 0 ? 
            categoryCounts.reduce((sum, count) => sum + count, 0) / totalCategories : 0;
        
        const mostPopularCategory = this.mockCategories.find(c => c.count === maxArticlesInCategory);
        
        const categoryDetails = this.mockCategories
            .sort((a, b) => b.count - a.count)
            .map(category => ({
                id: category.id,
                name: category.name,
                count: category.count,
                color: category.color,
                icon: category.icon
            }));
        
        return {
            success: true,
            data: {
                totalCategories,
                visibleCategories,
                emptyCategoriesCount,
                maxArticlesInCategory,
                averageArticlesPerCategory,
                mostPopularCategory: mostPopularCategory ? mostPopularCategory.name : '無',
                categoryDetails
            }
        };
    }

    // 建立分類
    async createCategory(categoryData) {
        await this.delay();
        
        // 檢查分類名稱是否已存在
        const existingCategory = this.mockCategories.find(c => 
            c.name.toLowerCase() === categoryData.name.toLowerCase()
        );
        
        if (existingCategory) {
            return {
                success: false,
                error: '分類名稱已存在'
            };
        }
        
        // 檢查代碼是否已存在
        if (categoryData.slug) {
            const existingSlug = this.mockCategories.find(c => 
                c.slug === categoryData.slug
            );
            
            if (existingSlug) {
                return {
                    success: false,
                    error: '分類代碼已存在'
                };
            }
        }
        
        const newCategory = {
            id: Math.max(...this.mockCategories.map(c => c.id)) + 1,
            ...categoryData,
            count: 0,
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
        };
        
        this.mockCategories.push(newCategory);
        
        return {
            success: true,
            data: newCategory,
            message: '分類建立成功'
        };
    }

    // 更新分類
    async updateCategory(id, categoryData) {
        await this.delay();
        
        const index = this.mockCategories.findIndex(c => c.id == id);
        
        if (index === -1) {
            return {
                success: false,
                error: '分類不存在'
            };
        }
        
        const currentCategory = this.mockCategories[index];
        
        // 檢查分類名稱是否與其他分類重複
        if (categoryData.name && categoryData.name !== currentCategory.name) {
            const existingCategory = this.mockCategories.find(c => 
                c.id != id && c.name.toLowerCase() === categoryData.name.toLowerCase()
            );
            
            if (existingCategory) {
                return {
                    success: false,
                    error: '分類名稱已存在'
                };
            }
        }
        
        // 檢查代碼是否與其他分類重複
        if (categoryData.slug && categoryData.slug !== currentCategory.slug) {
            const existingSlug = this.mockCategories.find(c => 
                c.id != id && c.slug === categoryData.slug
            );
            
            if (existingSlug) {
                return {
                    success: false,
                    error: '分類代碼已存在'
                };
            }
        }
        
        // 如果分類名稱改變，需要更新相關文章
        if (categoryData.name && categoryData.name !== currentCategory.name) {
            this.mockArticles.forEach(article => {
                if (article.category === currentCategory.name) {
                    article.category = categoryData.name;
                    article.updatedAt = new Date().toISOString();
                }
            });
        }
        
        this.mockCategories[index] = {
            ...currentCategory,
            ...categoryData,
            updatedAt: new Date().toISOString()
        };
        
        return {
            success: true,
            data: this.mockCategories[index],
            message: '分類更新成功'
        };
    }

    // 刪除分類
    async deleteCategory(id) {
        await this.delay();
        
        const index = this.mockCategories.findIndex(c => c.id == id);
        
        if (index === -1) {
            return {
                success: false,
                error: '分類不存在'
            };
        }
        
        const category = this.mockCategories[index];
        
        // 檢查是否有文章使用此分類
        const articlesInCategory = this.mockArticles.filter(
            article => article.category === category.name
        );
        
        // 將使用此分類的文章設為未分類
        if (articlesInCategory.length > 0) {
            articlesInCategory.forEach(article => {
                article.category = '未分類';
                article.updatedAt = new Date().toISOString();
            });
        }
        
        this.mockCategories.splice(index, 1);
        
        return {
            success: true,
            message: `分類刪除成功${articlesInCategory.length > 0 ? `，${articlesInCategory.length} 篇文章已移至未分類` : ''}`
        };
    }

    // 匯入分類
    async importCategories(categories) {
        await this.delay();
        
        let importedCount = 0;
        let skippedCount = 0;
        const errors = [];
        
        for (const categoryData of categories) {
            try {
                // 檢查必要欄位
                if (!categoryData.name) {
                    errors.push(`分類缺少名稱欄位`);
                    skippedCount++;
                    continue;
                }
                
                // 檢查是否已存在
                const existingCategory = this.mockCategories.find(c => 
                    c.name.toLowerCase() === categoryData.name.toLowerCase()
                );
                
                if (existingCategory) {
                    skippedCount++;
                    continue;
                }
                
                // 建立新分類
                const newCategory = {
                    id: Math.max(...this.mockCategories.map(c => c.id)) + 1,
                    name: categoryData.name,
                    slug: categoryData.slug || this.generateSlug(categoryData.name),
                    description: categoryData.description || '',
                    color: categoryData.color || '#007bff',
                    icon: categoryData.icon || '📁',
                    order: categoryData.order || 0,
                    visible: categoryData.visible !== false,
                    count: 0,
                    createdAt: new Date().toISOString(),
                    updatedAt: new Date().toISOString()
                };
                
                this.mockCategories.push(newCategory);
                importedCount++;
                
            } catch (error) {
                errors.push(`處理分類 "${categoryData.name || '未知'}" 時發生錯誤: ${error.message}`);
                skippedCount++;
            }
        }
        
        let message = `匯入完成：成功 ${importedCount} 個`;
        if (skippedCount > 0) {
            message += `，跳過 ${skippedCount} 個`;
        }
        
        return {
            success: true,
            message,
            data: {
                importedCount,
                skippedCount,
                errors
            }
        };
    }

    // 生成代碼的輔助方法
    generateSlug(name) {
        return name
            .toLowerCase()
            .replace(/[^\w\s-]/g, '')
            .replace(/[\s_-]+/g, '-')
            .replace(/^-+|-+$/g, '');
    }
}

// 建立全域 API 實例
window.adminAPI = new AdminAPI(); 