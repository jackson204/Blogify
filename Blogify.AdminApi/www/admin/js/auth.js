// 管理端認證邏輯
class AdminAuth {
    constructor() {
        this.init();
    }

    init() {
        // 檢查當前頁面是否需要認證
        const currentPage = window.location.pathname;
        const isLoginPage = currentPage.includes('login.html');
        
        if (isLoginPage) {
            this.initLoginPage();
        } else {
            this.checkAuthentication();
        }
    }

    initLoginPage() {
        // 如果已經登入，重導向到管理首頁
        if (adminAPI.isAuthenticated()) {
            window.location.href = 'index.html';
            return;
        }

        this.bindLoginEvents();
    }

    bindLoginEvents() {
        const loginForm = document.getElementById('loginForm');
        const fillDemoBtn = document.getElementById('fillDemoBtn');

        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleLogin();
            });
        }

        if (fillDemoBtn) {
            fillDemoBtn.addEventListener('click', () => {
                this.fillDemoCredentials();
            });
        }

        // Enter 鍵登入
        document.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.target.closest('form')) {
                const loginBtn = document.getElementById('loginBtn');
                if (loginBtn) {
                    loginBtn.click();
                }
            }
        });
    }

    fillDemoCredentials() {
        const usernameInput = document.getElementById('username');
        const passwordInput = document.getElementById('password');
        
        if (usernameInput && passwordInput) {
            usernameInput.value = 'admin';
            passwordInput.value = 'password';
            usernameInput.focus();
        }
    }

    async handleLogin() {
        const usernameInput = document.getElementById('username');
        const passwordInput = document.getElementById('password');
        const rememberMeInput = document.getElementById('rememberMe');
        const loginBtn = document.getElementById('loginBtn');
        const btnText = loginBtn.querySelector('.btn-text');
        const btnLoading = loginBtn.querySelector('.btn-loading');

        if (!usernameInput || !passwordInput) {
            Utils.showNotification('表單元素不存在', 'error');
            return;
        }

        const username = usernameInput.value.trim();
        const password = passwordInput.value.trim();
        const rememberMe = rememberMeInput ? rememberMeInput.checked : false;

        // 驗證輸入
        if (!username) {
            Utils.showNotification('請輸入帳號', 'warning');
            usernameInput.focus();
            return;
        }

        if (!password) {
            Utils.showNotification('請輸入密碼', 'warning');
            passwordInput.focus();
            return;
        }

        // 顯示載入狀態
        this.setLoginLoading(true);

        try {
            const result = await adminAPI.login(username, password);

            if (result.success) {
                // 記住我功能
                if (rememberMe) {
                    Utils.setStorage('rememberAdmin', true);
                }

                Utils.showNotification('登入成功！正在跳轉...', 'success');
                
                // 延遲跳轉，讓使用者看到成功訊息
                setTimeout(() => {
                    window.location.href = 'index.html';
                }, 1000);
            } else {
                Utils.showNotification(result.error || '登入失敗', 'error');
                passwordInput.value = '';
                passwordInput.focus();
            }
        } catch (error) {
            console.error('登入錯誤:', error);
            Utils.showNotification('登入過程發生錯誤，請稍後再試', 'error');
        } finally {
            this.setLoginLoading(false);
        }
    }

    setLoginLoading(loading) {
        const loginBtn = document.getElementById('loginBtn');
        const btnText = loginBtn?.querySelector('.btn-text');
        const btnLoading = loginBtn?.querySelector('.btn-loading');

        if (!loginBtn || !btnText || !btnLoading) return;

        if (loading) {
            loginBtn.disabled = true;
            btnText.classList.add('d-none');
            btnLoading.classList.remove('d-none');
        } else {
            loginBtn.disabled = false;
            btnText.classList.remove('d-none');
            btnLoading.classList.add('d-none');
        }
    }

    checkAuthentication() {
        if (!adminAPI.isAuthenticated()) {
            // 未登入，重導向到登入頁
            window.location.href = 'login.html';
            return false;
        }

        // 已登入，初始化管理介面
        this.initAdminInterface();
        return true;
    }

    initAdminInterface() {
        // 顯示使用者資訊
        this.displayUserInfo();
        
        // 綁定登出事件
        this.bindLogoutEvents();
        
        // 設定導航高亮
        this.setActiveNavigation();
    }

    displayUserInfo() {
        const user = adminAPI.getCurrentUser();
        if (!user) return;

        // 更新使用者頭像
        const userAvatar = document.querySelector('.user-avatar');
        if (userAvatar) {
            userAvatar.textContent = user.name.charAt(0).toUpperCase();
        }

        // 更新使用者名稱
        const userName = document.querySelector('.user-name');
        if (userName) {
            userName.textContent = user.name;
        }

        // 更新歡迎訊息
        const welcomeMessage = document.querySelector('.welcome-message');
        if (welcomeMessage) {
            const currentHour = new Date().getHours();
            let greeting = '早安';
            if (currentHour >= 12 && currentHour < 18) {
                greeting = '午安';
            } else if (currentHour >= 18) {
                greeting = '晚安';
            }
            welcomeMessage.textContent = `${greeting}，${user.name}！`;
        }
    }

    bindLogoutEvents() {
        const logoutBtns = document.querySelectorAll('.logout-btn, [data-action="logout"]');
        
        logoutBtns.forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.preventDefault();
                this.handleLogout();
            });
        });
    }

    async handleLogout() {
        const confirmed = confirm('確定要登出嗎？');
        if (!confirmed) return;

        try {
            const result = await adminAPI.logout();
            
            if (result.success) {
                Utils.showNotification('已成功登出', 'success');
                
                // 清除記住我設定
                Utils.removeStorage('rememberAdmin');
                
                // 跳轉到登入頁
                setTimeout(() => {
                    window.location.href = 'login.html';
                }, 500);
            } else {
                Utils.showNotification('登出失敗', 'error');
            }
        } catch (error) {
            console.error('登出錯誤:', error);
            Utils.showNotification('登出過程發生錯誤', 'error');
        }
    }

    setActiveNavigation() {
        const currentPage = window.location.pathname;
        const navLinks = document.querySelectorAll('.sidebar-menu a');
        
        navLinks.forEach(link => {
            link.classList.remove('active');
            
            const href = link.getAttribute('href');
            if (href && currentPage.includes(href)) {
                link.classList.add('active');
            }
        });

        // 如果沒有匹配的連結，預設選中第一個（通常是儀表板）
        const activeLink = document.querySelector('.sidebar-menu a.active');
        if (!activeLink) {
            const firstLink = document.querySelector('.sidebar-menu a');
            if (firstLink) {
                firstLink.classList.add('active');
            }
        }
    }

    // 權限檢查方法
    hasPermission(permission) {
        const user = adminAPI.getCurrentUser();
        if (!user) return false;

        // 簡單的權限檢查，實際應用中可能更複雜
        return user.permissions ? user.permissions.includes(permission) : true;
    }

    // 檢查是否為超級管理員
    isSuperAdmin() {
        const user = adminAPI.getCurrentUser();
        return user && user.role === 'super_admin';
    }

    // 自動登出（當 token 過期時）
    autoLogout() {
        Utils.showNotification('登入已過期，請重新登入', 'warning');
        setTimeout(() => {
            window.location.href = 'login.html';
        }, 2000);
    }

    // 刷新認證狀態
    refreshAuth() {
        if (!adminAPI.isAuthenticated()) {
            this.autoLogout();
            return false;
        }
        return true;
    }
}

// 頁面載入完成後初始化認證
document.addEventListener('DOMContentLoaded', () => {
    window.adminAuth = new AdminAuth();
});

// 全域錯誤處理
window.addEventListener('unhandledrejection', (event) => {
    if (event.reason && event.reason.status === 401) {
        // 認證失敗，自動登出
        if (window.adminAuth) {
            window.adminAuth.autoLogout();
        }
    }
}); 