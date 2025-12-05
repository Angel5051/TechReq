using TechReq.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TechReq.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Проверяем, создана ли БД
            context.Database.EnsureCreated();

            // Если услуги уже есть, ничего не делаем
            if (context.Services.Any())
            {
                return;
            }

            var services = new List<ServiceDb>();

            // --- 1. ПЛАНОВОЕ ТО (MAINTENANCE) ---
            services.AddRange(new[]
            {
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена моторного масла", Description = "Полная замена масла и масляного фильтра.", Price = 800, Category = "maintenance", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена воздушного фильтра", Description = "Замена фильтрующего элемента двигателя.", Price = 300, Category = "maintenance", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена салонного фильтра", Description = "Очистка воздуха в салоне авто.", Price = 400, Category = "maintenance", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена топливного фильтра", Description = "Замена фильтра тонкой очистки топлива.", Price = 1200, Category = "maintenance", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена свечей зажигания", Description = "Комплект 4 шт. (без учета стоимости свечей).", Price = 1000, Category = "maintenance", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена антифриза", Description = "Аппаратная замена охлаждающей жидкости.", Price = 1500, Category = "maintenance", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена тормозной жидкости", Description = "Прокачка тормозной системы.", Price = 1200, Category = "maintenance", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена масла в АКПП", Description = "Частичная замена масла в коробке.", Price = 2500, Category = "maintenance", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена ремня ГРМ", Description = "Замена ремня газораспределительного механизма.", Price = 5000, Category = "maintenance", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Заправка кондиционера", Description = "Полный цикл заправки фреоном.", Price = 2000, Category = "maintenance", IsExpress = true }
            });

            // --- 2. ДИАГНОСТИКА (DIAGNOSTICS) ---
            services.AddRange(new[]
            {
                new ServiceDb { Id = Guid.NewGuid(), Name = "Компьютерная диагностика", Description = "Считывание и расшифровка кодов ошибок.", Price = 1000, Category = "diagnostics", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Диагностика подвески", Description = "Осмотр ходовой части на подъемнике.", Price = 500, Category = "diagnostics", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Эндоскопия двигателя", Description = "Осмотр цилиндров камерой на задиры.", Price = 2500, Category = "diagnostics", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замер компрессии", Description = "Измерение давления в цилиндрах двигателя.", Price = 1200, Category = "diagnostics", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Диагностика тормозной системы", Description = "Проверка износа колодок и дисков.", Price = 600, Category = "diagnostics", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Проверка аккумулятора", Description = "Тест емкости и пускового тока.", Price = 300, Category = "diagnostics", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Диагностика автоэлектрики", Description = "Поиск утечек тока и неисправностей.", Price = 1500, Category = "diagnostics", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Осмотр перед покупкой", Description = "Комплексная проверка авто для покупателя.", Price = 3500, Category = "diagnostics", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Проверка ЛКП", Description = "Замер толщины краски толщиномером.", Price = 1000, Category = "diagnostics", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Диагностика выхлопной системы", Description = "Поиск прогоревших элементов.", Price = 500, Category = "diagnostics", IsExpress = true }
            });

            // --- 3. РЕМОНТ ХОДОВОЙ (CHASSIS) ---
            services.AddRange(new[]
            {
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена передних колодок", Description = "Работа по замене тормозных колодок.", Price = 800, Category = "chassis", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена задних колодок", Description = "Дисковые или барабанные тормоза.", Price = 1000, Category = "chassis", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена амортизаторов", Description = "Замена стойки амортизатора (1 шт).", Price = 1800, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена шаровой опоры", Description = "Без снятия рычага.", Price = 900, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена ступичного подшипника", Description = "Перепрессовка подшипника ступицы.", Price = 1600, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена рулевого наконечника", Description = "С последующей регулировкой схождения.", Price = 700, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена сайлентблоков", Description = "Перепрессовка сайлентблоков рычага.", Price = 1500, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Развал-схождение (1 ось)", Description = "Регулировка углов установки колес.", Price = 1200, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Развал-схождение (2 оси)", Description = "Полная регулировка геометрии.", Price = 1800, Category = "chassis", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена ШРУСа", Description = "Замена гранаты привода.", Price = 1500, Category = "chassis", IsExpress = false }
            });

            // --- 4. КУЗОВНОЙ РЕМОНТ (BODYWORK) ---
            services.AddRange(new[]
            {
                new ServiceDb { Id = Guid.NewGuid(), Name = "Покраска бампера", Description = "Локальная или полная покраска.", Price = 6000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Покраска крыла", Description = "Снятие, покраска, установка.", Price = 7000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Полировка фар", Description = "Восстановление прозрачности оптики.", Price = 1500, Category = "bodywork", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Абразивная полировка кузова", Description = "Удаление мелких царапин.", Price = 12000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Удаление вмятин без покраски", Description = "Технология PDR (за 1 вмятину).", Price = 1500, Category = "bodywork", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Замена лобового стекла", Description = "Вклейка нового стекла.", Price = 3000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Ремонт пластика", Description = "Пайка трещин на бамперах.", Price = 2000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Антикоррозийная обработка", Description = "Обработка днища и арок.", Price = 8000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Керамическое покрытие", Description = "Нанесение защитного состава.", Price = 15000, Category = "bodywork", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Шумоизоляция дверей", Description = "Проклейка вибро- и шумоизоляцией.", Price = 4000, Category = "bodywork", IsExpress = false }
            });

            // --- 5. ШИНОМОНТАЖ (TIRES) ---
            services.AddRange(new[]
            {
                new ServiceDb { Id = Guid.NewGuid(), Name = "Комплекс R13-R14", Description = "Снятие, монтаж, балансировка, установка.", Price = 1400, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Комплекс R15-R16", Description = "Снятие, монтаж, балансировка, установка.", Price = 1600, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Комплекс R17-R18", Description = "Снятие, монтаж, балансировка, установка.", Price = 2000, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Комплекс R19+", Description = "Снятие, монтаж, балансировка, установка.", Price = 2500, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Балансировка колеса", Description = "Устранение биения (за 1 шт).", Price = 200, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Ремонт прокола (жгут)", Description = "Быстрый ремонт без разбортировки.", Price = 300, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Ремонт прокола (латка)", Description = "Ремонт с разбортировкой колеса.", Price = 600, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Правка литого диска", Description = "Восстановление геометрии диска.", Price = 1200, Category = "tires", IsExpress = false },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Сезонное хранение шин", Description = "Хранение комплекта (6 месяцев).", Price = 2000, Category = "tires", IsExpress = true },
                new ServiceDb { Id = Guid.NewGuid(), Name = "Чернение резины", Description = "Обработка шин защитным составом.", Price = 200, Category = "tires", IsExpress = true }
            });

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}