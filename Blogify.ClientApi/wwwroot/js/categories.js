// 分類編輯頁面 JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // 表單驗證
    const categoryForm = document.getElementById('categoryForm');
    if (categoryForm) {
        categoryForm.addEventListener('submit', function(e) {
            const nameInput = document.getElementById('categoryName') || document.querySelector('input[name="Name"]');
            
            if (nameInput && !nameInput.value.trim()) {
                e.preventDefault();
                alert('請輸入分類名稱');
                nameInput.focus();
                return false;
            }
        });
    }

    // 自動調整文字區域高度
    const descriptionTextarea = document.querySelector('textarea[name="Description"]');
    if (descriptionTextarea) {
        descriptionTextarea.addEventListener('input', function() {
            this.style.height = 'auto';
            this.style.height = this.scrollHeight + 'px';
        });
    }

    // 確認刪除操作
    const deleteButtons = document.querySelectorAll('form[action*="Delete"] button[type="submit"]');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            const categoryName = this.closest('tr').querySelector('td:first-child strong').textContent;
            if (!confirm(`確定要刪除分類「${categoryName}」嗎？\n\n注意：刪除分類可能會影響相關的文章。`)) {
                e.preventDefault();
                return false;
            }
        });
    });
});
