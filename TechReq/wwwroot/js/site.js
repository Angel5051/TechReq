(function () {
    // =========================================================
    // 1. ГЛОБАЛЬНЫЕ НАСТРОЙКИ И TOASTS
    // =========================================================

    const modalSelector = '#authModal';
    const iosModalSelector = '#iosConfirmModal';

    // --- Логика Toast (Уведомлений) ---
    // (Предполагаем, что CSS для #toast-container и .toast существует)
    window.showToast = function (message, type = 'info') {
        const toastContainer = document.getElementById('toast-container');
        if (!toastContainer) return; // Выходим, если контейнер не найден

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        toast.innerHTML = `<span>${message}</span><button onclick="this.parentElement.remove()">✕</button>`;

        toastContainer.appendChild(toast);
        requestAnimationFrame(() => toast.classList.add('show'));

        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    };


    // =========================================================
    // 2. ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
    // =========================================================

    // --- Функция открытия/закрытия любых модалок ---
    function toggleDisplay(selector) {
        let x = document.querySelector(selector);
        if (!x) return;

        const isHidden = getComputedStyle(x).display === 'none' || getComputedStyle(x).display === '';

        if (isHidden) {
            x.style.display = 'flex';
            document.body.style.overflow = 'hidden';

            // Закрываем мобильное меню, если оно было открыто
            const mobilePanel = document.getElementById('mobilePanel');
            if (mobilePanel && getComputedStyle(mobilePanel).display !== 'none') {
                mobilePanel.style.display = 'none';
            }
        } else {
            x.style.display = 'none';
            document.body.style.overflow = '';
        }
    }

    // --- Функция очистки и закрытия формы ---
    function cleaningAndClosingForm(form, errorContainer, modalSelector) {
        if (errorContainer) errorContainer.innerHTML = '';
        if (form) form.reset();

        const modal = document.querySelector(modalSelector);
        if (modal) {
            modal.style.display = 'none';
            document.body.style.overflow = '';
        }
    }

    // --- AJAX Запрос (ИСПРАВЛЕНО: Корректная обработка ошибок) ---
    async function sendRequest(method, url, data) {
        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        // Считываем JSON ответ (даже если это ошибка)
        const responseData = await response.json();

        // ЕСЛИ СТАТУС НЕ 200/201, ИЛИ success: false
        if (!response.ok || responseData.success === false || responseData.statusCode >= 400 || responseData.StatusCode >= 400) {
            throw responseData; // Бросаем объект с ошибкой для блока .catch
        }

        return responseData;
    }

    // --- ГЛОБАЛЬНАЯ ФУНКЦИЯ ОТКРЫТИЯ МОДАЛЬНОГО ОКНА ---
    window.openAuthModal = function (targetTab = 'login') {
        const modal = document.querySelector(modalSelector);
        if (!modal) return;

        // Устанавливаем активный таб и форму вручную
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.tab-form').forEach(f => f.classList.remove('active'));

        const targetBtn = document.querySelector(`.tab-btn[data-tab="${targetTab}"]`);
        const targetForm = document.getElementById(`form_${targetTab}`);

        if (targetBtn) targetBtn.classList.add('active');
        if (targetForm) targetForm.classList.add('active');

        // Открытие модального окна
        modal.style.display = 'flex';
        document.body.style.overflow = 'hidden';
    };


    document.addEventListener('DOMContentLoaded', () => {

        // =========================================================
        // 3. ПРИВЯЗКА СОБЫТИЙ (HEADER, TABS)
        // =========================================================

        // Обработчики открытия модального окна (Header, Mobile Menu)
        const openLogin = (e) => {
            if (e) e.preventDefault();
            window.openAuthModal('login');
        };

        document.getElementById('authBtn')?.addEventListener('click', openLogin);
        document.getElementById('mobileLogin')?.addEventListener('click', openLogin);
        document.getElementById('hamburger')?.addEventListener('click', () => toggleDisplay('#mobilePanel'));
        document.getElementById('guestBtn')?.addEventListener('click', () => toggleDisplay('#mobilePanel'));


        // Закрытие по крестику
        document.querySelector('.close')?.addEventListener('click', () => {
            cleaningAndClosingForm(document.getElementById('form_login'), document.getElementById('error-messages-login'), modalSelector);
        });

        // Закрытие по клику на фон
        window.addEventListener('click', (e) => {
            const modal = document.querySelector(modalSelector);
            if (e.target === modal) {
                cleaningAndClosingForm(null, null, modalSelector);
            }
        });


        // Переключение табов внутри модального окна
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.preventDefault();
                const targetTab = btn.dataset.tab;
                window.openAuthModal(targetTab);
            });
        });


        // =========================================================
        // 4. ЛОГИКА ВХОДА (LOGIN) - ИСПРАВЛЕНО
        // =========================================================
        const loginForm = document.getElementById('form_login');
        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                const formData = {
                    Email: loginForm.querySelector('input[name="Email"]')?.value,
                    Password: loginForm.querySelector('input[name="Password"]')?.value
                };
                const errorCont = document.getElementById('error-messages-login');
                errorCont.innerHTML = '';

                sendRequest('POST', '/Home/Login', formData)
                    .then(result => {
                        // УСПЕХ: Вход выполнен
                        window.showToast('Вход выполнен!', 'success');
                        cleaningAndClosingForm(loginForm, errorCont, modalSelector);

                        // ПЕРЕЗАГРУЗКА ДЛЯ АВТОРИЗАЦИИ
                        setTimeout(() => { window.location.reload(); }, 300);
                    })
                    .catch(error => {
                        // ОШИБКА: Показываем сообщение из сервера
                        const errorArray = error.errors || [error.description || error.Description || 'Ошибка входа'];

                        window.showToast(errorArray[0], 'error');
                        errorCont.innerHTML = errorArray.map(msg => `<div class="error" style="color:red; font-size:13px;">${msg}</div>`).join('');
                    });
            });
        }

        // =========================================================
        // 5. ЛОГИКА РЕГИСТРАЦИИ (REGISTER)
        // =========================================================
        const regForm = document.getElementById('form_register');
        if (regForm) {
            regForm.addEventListener('submit', (e) => {
                e.preventDefault();

                const formData = {
                    Login: regForm.querySelector('input[name="Login"]')?.value,
                    Email: regForm.querySelector('input[name="Email"]')?.value,
                    Password: regForm.querySelector('input[name="Password"]')?.value,
                    PasswordConfirm: regForm.querySelector('input[name="PasswordConfirm"]')?.value
                };

                const errorCont = document.getElementById('error-messages-register');
                errorCont.innerHTML = '';

                if (formData.Password !== formData.PasswordConfirm) {
                    window.showToast('Пароли не совпадают', 'error');
                    errorCont.innerHTML = `<div class="error" style="color:red; font-size:13px;">Пароли не совпадают</div>`;
                    return;
                }

                window.showToast('Отправка кода...', 'info');

                sendRequest('POST', '/Home/Register', formData)
                    .then(data => {
                        window.showToast('Код отправлен! Проверьте почту.', 'success');
                        cleaningAndClosingForm(regForm, errorCont, modalSelector);

                        localStorage.setItem('regEmail', formData.Email);

                        // Открываем модалку подтверждения
                        toggleDisplay(iosModalSelector);

                        confirmEmail(formData);
                    })
                    .catch(error => {
                        // ОШИБКА
                        const errorArray = error.errors || [error.description || error.Description || 'Неизвестная ошибка регистрации'];
                        window.showToast(errorArray[0], 'error');
                        errorCont.innerHTML = errorArray.map(msg => `<div class="error" style="color:red; font-size:13px;">${msg}</div>`).join('');
                    });
            });
        }

        // =========================================================
        // 6. ЛОГИКА ПОДТВЕРЖДЕНИЯ (CONFIRM EMAIL)
        // =========================================================
        function confirmEmail(userDataBody) {
            const sendConfirmBtn = document.getElementById('btnSendCode');
            const closeConfirmBtn = document.getElementById('btnCloseCode');

            // --- Перепривязка обработчиков для корректной работы ---
            const cloneSend = sendConfirmBtn.cloneNode(true);
            const cloneClose = closeConfirmBtn.cloneNode(true);
            sendConfirmBtn.parentNode.replaceChild(cloneSend, sendConfirmBtn);
            closeConfirmBtn.parentNode.replaceChild(cloneClose, closeConfirmBtn);

            // Кнопка "ОК" (Отправить код)
            cloneSend.addEventListener('click', function () {
                const inputCode = document.getElementById('codeData').value;
                const email = localStorage.getItem('regEmail');

                const body = {
                    Email: email,
                    ConfirmCode: inputCode,
                };

                sendRequest('POST', '/Home/ConfirmEmail', body)
                    .then(data => {
                        window.showToast('Регистрация завершена!', 'success');
                        toggleDisplay(iosModalSelector);
                        localStorage.removeItem('regEmail');

                        // ПЕРЕЗАГРУЗКА ДЛЯ АВТОРИЗАЦИИ
                        setTimeout(() => { window.location.reload(); }, 300);
                    })
                    .catch(error => {
                        window.showToast('Неверный код или ошибка сервера', 'error');
                    });
            });

            // Кнопка "Закрыть/Отмена"
            cloneClose.addEventListener('click', function () {
                toggleDisplay(iosModalSelector);
                window.showToast('Регистрация прервана', 'info');
                localStorage.removeItem('regEmail');
            });
        }

        // =========================================================
        // 7. GOOGLE AUTH
        // =========================================================
        document.querySelectorAll('.google-btn').forEach(btn => {
            btn.addEventListener('click', function (e) {
                window.location.href = `/Home/AuthenticationGoogle?returnUrl=${encodeURIComponent(window.location.href)}`;
            });
        });

    });
})();