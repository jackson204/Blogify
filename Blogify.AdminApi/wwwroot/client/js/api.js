// 客戶端 API 模擬
class ClientAPI {
    constructor() {
        this.baseURL = '/api';
        this.initMockData();
    }

    // 初始化假資料
    initMockData() {
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
                image: "https://via.placeholder.com/800x400/007bff/ffffff?text=JavaScript+ES6%2B"
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
                image: "https://via.placeholder.com/800x400/28a745/ffffff?text=CSS+Grid+vs+Flexbox"
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
                image: "https://via.placeholder.com/800x400/ffc107/000000?text=Node.js+Performance"
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
                image: "https://via.placeholder.com/800x400/61dafb/000000?text=React+Hooks"
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
                image: "https://via.placeholder.com/800x400/4db33d/ffffff?text=MongoDB+Best+Practices"
            }
        ];

        this.mockCategories = [
            { id: 1, name: "前端開發", count: 3 },
            { id: 2, name: "後端開發", count: 1 },
            { id: 3, name: "資料庫", count: 1 },
            { id: 4, name: "DevOps", count: 0 },
            { id: 5, name: "行動開發", count: 0 }
        ];
    }

    // 模擬 API 延遲
    async delay(ms = 500) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    // 獲取文章列表
    async getArticles(params = {}) {
        await this.delay();
        
        let articles = [...this.mockArticles];
        
        // 分類篩選
        if (params.category) {
            articles = articles.filter(article => 
                article.category === params.category
            );
        }
        
        // 搜尋
        if (params.search) {
            const searchTerm = params.search.toLowerCase();
            articles = articles.filter(article =>
                article.title.toLowerCase().includes(searchTerm) ||
                article.content.toLowerCase().includes(searchTerm) ||
                article.tags.some(tag => tag.toLowerCase().includes(searchTerm))
            );
        }
        
        // 排序
        if (params.sort === 'date') {
            articles.sort((a, b) => new Date(b.publishDate) - new Date(a.publishDate));
        } else if (params.sort === 'views') {
            articles.sort((a, b) => b.views - a.views);
        } else if (params.sort === 'likes') {
            articles.sort((a, b) => b.likes - a.likes);
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

    // 獲取單篇文章
    async getArticle(id) {
        await this.delay();
        
        const article = this.mockArticles.find(a => a.id == id);
        
        if (!article) {
            return {
                success: false,
                error: '文章不存在'
            };
        }
        
        // 模擬增加瀏覽次數
        article.views += 1;
        
        return {
            success: true,
            data: article
        };
    }

    // 獲取分類列表
    async getCategories() {
        await this.delay(200);
        
        return {
            success: true,
            data: this.mockCategories
        };
    }

    // 獲取熱門文章
    async getPopularArticles(limit = 5) {
        await this.delay(300);
        
        const popular = [...this.mockArticles]
            .sort((a, b) => b.views - a.views)
            .slice(0, limit)
            .map(article => ({
                id: article.id,
                title: article.title,
                views: article.views,
                publishDate: article.publishDate
            }));
        
        return {
            success: true,
            data: popular
        };
    }

    // 獲取最新文章
    async getLatestArticles(limit = 5) {
        await this.delay(300);
        
        const latest = [...this.mockArticles]
            .sort((a, b) => new Date(b.publishDate) - new Date(a.publishDate))
            .slice(0, limit)
            .map(article => ({
                id: article.id,
                title: article.title,
                publishDate: article.publishDate,
                author: article.author
            }));
        
        return {
            success: true,
            data: latest
        };
    }

    // 點讚文章
    async likeArticle(id) {
        await this.delay(200);
        
        const article = this.mockArticles.find(a => a.id == id);
        
        if (!article) {
            return {
                success: false,
                error: '文章不存在'
            };
        }
        
        // 檢查是否已經點讚
        const likedArticles = Utils.getStorage('likedArticles') || [];
        const hasLiked = likedArticles.includes(id);
        
        if (hasLiked) {
            return {
                success: false,
                error: '您已經點讚過這篇文章'
            };
        }
        
        // 增加點讚數
        article.likes += 1;
        
        // 記錄點讚狀態
        likedArticles.push(id);
        Utils.setStorage('likedArticles', likedArticles);
        
        return {
            success: true,
            data: {
                likes: article.likes,
                message: '點讚成功！'
            }
        };
    }

    // 檢查點讚狀態
    hasLiked(id) {
        const likedArticles = Utils.getStorage('likedArticles') || [];
        return likedArticles.includes(id);
    }
}

// 建立全域 API 實例
window.clientAPI = new ClientAPI(); 