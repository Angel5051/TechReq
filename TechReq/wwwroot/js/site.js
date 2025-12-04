(function () {
    // =========================================================
    // 1. СТИЛИ И ЛОГИКА УВЕДОМЛЕНИЙ (TOAST)
    // =========================================================
    const STYLE_ID = 'site-toasts-styles';
    if (!document.getElementById(STYLE_ID)) {
        const css = `
            #toast-container { position: fixed; bottom: 20px; left: 50%; transform: translateX(-50%); display: flex; flex-direction: column; gap: 10px; z-index: 999999; pointer-events: none; }
            .toast { pointer-events: auto; padding: 12px 16px; min-width: 250px; border-radius: 8px; color: #fff; font-size: 14px; background: rgba(0,0,0,0.85); backdrop-filter: blur(4px); opacity: 0; transform: translateY(20px); transition: all 0.3s ease; display: flex; align-items: center; justify-content: space-between; box-shadow: 0 4px 12px rgba(0,0,0,0.3); }
            .toast.show { opacity: 1; transform: translateY(0); }
            .toast.success { border-left: 4px solid #2ecc71; }
            .toast.error { border-left: 4px solid #e74c3c; }
            .toast button { background: none; border: none; color: #fff; cursor: pointer; font-size: 16px; margin-left: 10px; }
        `;
        const style = document.createElement('style');
        style.id = STYLE_ID;
        style.textContent = css;
        document.head.appendChild(style);
    }

    let toastContainer = document.getElementById('toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toast-container';
        document.body.appendChild(toastContainer);
    }

    window.showToast = function (message, type = 'info') {
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

    document.addEventListener('DOMContentLoaded', () => {

        // =========================================================
        // 2. ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
        // =========================================================

        // --- Функция открытия/закрытия окон ---
        function hiddenOpen_Closeclick(selector) {
            let x = document.querySelector(selector);
            if (!x) return;

            if (getComputedStyle(x).display === 'none') {
                x.style.display = 'flex'; // Используем flex для центрирования
                document.body.style.overflow = 'hidden'; // Блокируем прокрутку фона

                // Если открываем модалку, закрываем мобильное меню
                const mobilePanel = document.getElementById('mobilePanel');
                if (mobilePanel && mobilePanel.style.display === 'block') {
                    mobilePanel.style.display = 'none';
                }
            } else {
                x.style.display = 'none';
                document.body.style.overflow = '';
            }
        }

        // --- Функция очистки и закрытия формы ---
        function cleaningAndClosingForm(form, errorContainer, modalSelector) {
            // Очистка ошибок
            if (errorContainer) {
                errorContainer.innerHTML = '';
            }
            // Сброс формы
            if (form) {
                form.reset();
            }
            // Закрытие окна
            hiddenOpen_Closeclick(modalSelector);
        }

        // --- AJAX Запрос ---
        async function sendRequest(method, url, data) {
            const response = await fetch(url, {
                method: method,
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            return await response.json();
        }

        // =========================================================
        // 3. ПРИВЯЗКА СОБЫТИЙ (КНОПКИ)
        // =========================================================

        const modalSelector = '#authModal';
        const iosModalSelector = '#iosConfirmModal';

        // Кнопки открытия модального окна
        const clickToHideBtn = document.getElementById('click-to-hide');
        const sideMenuBtn = document.getElementById('side-menu-button-click-to-hide');
        const authBtn = document.getElementById('authBtn');
        const mobileLoginBtn = document.getElementById('mobileLogin');

        const openFunc = () => hiddenOpen_Closeclick(modalSelector);

        if (clickToHideBtn) clickToHideBtn.addEventListener('click', openFunc);
        if (sideMenuBtn) sideMenuBtn.addEventListener('click', openFunc);
        if (authBtn) authBtn.addEventListener('click', openFunc);
        if (mobileLoginBtn) mobileLoginBtn.addEventListener('click', openFunc);

        // Закрытие по клику на фон
        window.addEventListener('click', (e) => {
            const modal = document.querySelector(modalSelector);
            if (e.target === modal) {
                hiddenOpen_Closeclick(modalSelector);
            }
        });

        // Закрытие по крестику
        const closeBtn = document.querySelector('.close');
        if (closeBtn) {
            closeBtn.addEventListener('click', () => {
                cleaningAndClosingForm(document.getElementById('form_login'), document.getElementById('error-messages-login'), modalSelector);
            });
        }

        // Переключение вкладок (Вход / Регистрация)
        const tabButtons = document.querySelectorAll('.tab-btn');
        const tabForms = document.querySelectorAll('.tab-form');
        tabButtons.forEach(btn => {
            btn.addEventListener('click', () => {
                tabButtons.forEach(b => b.classList.remove('active'));
                tabForms.forEach(f => f.classList.remove('active'));
                btn.classList.add('active');
                const targetId = `form_${btn.dataset.tab}`;
                const targetForm = document.getElementById(targetId);
                if (targetForm) targetForm.classList.add('active');
            });
        });

        // =========================================================
        // 4. ЛОГИКА ВХОДА (LOGIN)
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

                sendRequest('POST', '/Home/Login', formData)
                    .then(result => {
                        // Проверка успеха (учитываем регистр)
                        const isSuccess = result.statusCode === 200 || result.StatusCode === 200 || result.success === true;

                        if (isSuccess) {
                            showToast('Вход выполнен!', 'success');
                            cleaningAndClosingForm(loginForm, errorCont, modalSelector);
                            setTimeout(() => { window.location.reload(); }, 1000);
                        } else {
                            const msg = result.description || result.Description || (result.errors ? result.errors.join(', ') : 'Ошибка');
                            showToast(msg, 'error');
                            if (errorCont) errorCont.innerHTML = `<div style="color:red;">${msg}</div>`;
                        }
                    })
                    .catch(err => {
                        console.error(err);
                        showToast('Ошибка соединения', 'error');
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

                if (formData.Password !== formData.PasswordConfirm) {
                    showToast('Пароли не совпадают', 'error');
                    return;
                }

                showToast('Отправка кода...', 'info');

                sendRequest('POST', '/Home/Register', formData)
                    .then(data => {
                        console.log("Register response:", data);

                        // Проверка успеха
                        const isSuccess = data.statusCode === 200 || data.StatusCode === 200;

                        if (isSuccess) {
                            showToast('Код отправлен!', 'success');

                            // Сохраняем код
                            localStorage.setItem('serverCode', data.data || data.Data);

                            // 1. Очищаем и закрываем форму регистрации
                            cleaningAndClosingForm(regForm, errorCont, modalSelector);

                            // 2. Открываем окно подтверждения (iOS)
                            hiddenOpen_Closeclick(iosModalSelector);

                            // 3. Запускаем логику ожидания кода
                            confirmEmail(formData);
                        } else {
                            const msg = data.description || data.Description || 'Ошибка';
                            showToast(msg, 'error');
                            if (errorCont) errorCont.innerHTML = `<div style="color:red;">${msg}</div>`;
                        }
                    })
                    .catch(err => {
                        console.error(err);
                        showToast('Ошибка соединения', 'error');
                    });
            });
        }

        // =========================================================
        // 6. ЛОГИКА ПОДТВЕРЖДЕНИЯ (CONFIRM EMAIL)
        // =========================================================
        function confirmEmail(userDataBody) {
            const sendConfirmBtn = document.getElementById('btnSendCode');
            const closeConfirmBtn = document.getElementById('btnCloseCode');

            // Удаляем старые обработчики (клонированием)
            const newSendBtn = sendConfirmBtn.cloneNode(true);
            sendConfirmBtn.parentNode.replaceChild(newSendBtn, sendConfirmBtn);

            const newCloseBtn = closeConfirmBtn.cloneNode(true);
            closeConfirmBtn.parentNode.replaceChild(newCloseBtn, closeConfirmBtn);

            // Кнопка "ОК" (Отправить код)
            newSendBtn.addEventListener('click', function () {
                const inputCode = document.getElementById('codeData').value;
                const serverCode = localStorage.getItem('serverCode');

                const body = {
                    ...userDataBody,
                    Code: serverCode,
                    ConfirmCode: inputCode
                };

                sendRequest('POST', '/Home/ConfirmEmail', body)
                    .then(data => {
                        const isSuccess = data.statusCode === 200 || data.StatusCode === 200;

                        if (isSuccess) {
                            showToast('Регистрация завершена!', 'success');

                            // Закрываем окно
                            hiddenOpen_Closeclick(iosModalSelector);
                            localStorage.removeItem('serverCode');

                            // Перезагрузка страницы
                            window.location.reload();
                        } else {
                            showToast(data.description || data.Description || 'Неверный код', 'error');
                        }
                    })
                    .catch(err => {
                        console.error(err);
                        showToast('Ошибка сервера', 'error');
                    });
            });

            // Кнопка "Закрыть/Отмена"
            newCloseBtn.addEventListener('click', function () {
                hiddenOpen_Closeclick(iosModalSelector);
                showToast('Регистрация прервана', 'info');
            });
        }

        // =========================================================
        // 7. МОБИЛЬНОЕ МЕНЮ
        // =========================================================
        const hamburger = document.getElementById('hamburger');
        const guestBtn = document.getElementById('guestBtn');

        if (hamburger) hamburger.addEventListener('click', () => hiddenOpen_Closeclick('#mobilePanel'));
        if (guestBtn) guestBtn.addEventListener('click', () => hiddenOpen_Closeclick('#mobilePanel'));

        // =========================================================
        // 8. GOOGLE AUTH (ДОБАВЛЕНО)
        // =========================================================
        const googleBtns = document.querySelectorAll('.google-btn');

        if (googleBtns) {
            googleBtns.forEach(btn => {
                btn.addEventListener('click', function (e) {
                    // e.preventDefault(); // Если нужно отменить стандартный переход

                    // Перенаправление на метод контроллера
                    window.location.href = `/Home/AuthenticationGoogle?returnUrl=${encodeURIComponent(window.location.href)}`;
                });
            });
        }

    });
})();