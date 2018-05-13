using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma_Curator_Subsystem.Models;

namespace Diploma_Curator_Subsystem.Data
{
    public class DbInitializer
    {
        public static void Initialize(SubsystemContext context)
        {
            context.Database.EnsureCreated();
            if (context.Statuses.Any())
            {
                return;
            }

            //Статусы
            var statuses = new Status[]
            {
                new Status{Name="Открыта"},
                new Status{Name="Ожидает подбора группы от КЭС"},
                new Status{Name="Выполнена"}
            };
            foreach (Status s in statuses)
            {
                context.Statuses.Add(s);
            }
            context.SaveChanges();

            //Предметные области
            var domains = new Domain[]
            {
                new Domain{Name="Информационные технологии"},
                new Domain{Name="Строительство"},
                new Domain{Name="Машиностроение"}
            };
            foreach (Domain d in domains)
            {
                context.Domains.Add(d);
            }
            context.SaveChanges();

            //Роли
            var roles = new Role[]
            {
                new Role{Name="ЛПР", Description=""},
                new Role{Name="Главный менеджер", Description=""},
                new Role{Name="Координатор", Description=""},
                new Role{Name="Аналитик", Description=""},
                new Role{Name="Куратор экспертного сообщества", Description=""},
                new Role{Name="Эксперт", Description=""}
            };
            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            //Задачи
            var tasks = new Models.Task[]
            {
                new Models.Task{Title="Выбор СУБД",Description="Необходимо выбрать систему управления базой данных для проекта",Alternatives="4 альтернативы", Math_data="123", StatusID=2, DomainID=1},
                new Models.Task{Title="Выбор языка программирования",Description="Необходимо выбрать язык программирования для проекта",Alternatives="3 альтернативы", Math_data="123", StatusID=2, DomainID=1},
                new Models.Task{Title="Выбор технологии строительства",Description="Необходимо выбрать технологию строительства дома",Alternatives="5 альтернативы", Math_data="123", StatusID=1, DomainID=2},
                new Models.Task{Title="Выбор вида металла",Description="Необходимо выбрать вид металла для нового робота",Alternatives="6 альтернативы", Math_data="123", StatusID=2, DomainID=3},
                new Models.Task{Title="Выбор материалов строительства",Description="Необходимо выбрать материалы для строительства дома",Alternatives="2 альтернативы", Math_data="123", StatusID=3, DomainID=2}
            };
            foreach (Models.Task t in tasks)
            {
                context.Tasks.Add(t);
            }
            context.SaveChanges();

            //Пользователи
            var users = new User[]
            {
                new User{Name="Alyana",LastName="Matyusheva",Email="alk-fifa@yandex.ru", Password="123"},
                new User{Name="Grigorii",LastName="Praskura",Email="gregorypraskura@hotmail.com", Password="12345"},
                new User{Name="Ivan",LastName="Ivanov",Email="ivanov@ya.ru", Password="123"},
                new User{Name="Petr",LastName="Petrov",Email="petrov@yandex.ru", Password="123"},
                new User{Name="Sergey",LastName="Sidorov",Email="sidorov@yandex.ru", Password="123"},
                new User{Name="Dmitriy",LastName="Mikhailov",Email="mikhailov@yandex.ru", Password="123"},
                new User{Name="Альяна",LastName="Матюшева",Email="alk-fifa@yandex.ru", Password="123"},
                new User{Name="Григорий",LastName="Праскура",Email="gregorypraskura@hotmail.com", Password="12345"},
                new User{Name="Иван",LastName="Иванов",Email="ivanov@ya.ru", Password="123"},
                new User{Name="Петр",LastName="Петров",Email="petrov@yandex.ru", Password="123"},
                new User{Name="Сергей",LastName="Сидоров",Email="sidorov@yandex.ru", Password="123"},
                new User{Name="Дмитрий",LastName="Михайлов",Email="mikhailov@yandex.ru", Password="123"}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            //UserRole
            var userRoles = new UserRole[]
            {
                new UserRole{UserID=1,RoleID=6},
                new UserRole{UserID=2,RoleID=6},
                new UserRole{UserID=3,RoleID=6},
                new UserRole{UserID=4,RoleID=6},
                new UserRole{UserID=5,RoleID=6},
                new UserRole{UserID=6,RoleID=6},
                new UserRole{UserID=7,RoleID=6},
                new UserRole{UserID=8,RoleID=6},
                new UserRole{UserID=9,RoleID=6},
                new UserRole{UserID=10,RoleID=6},
                new UserRole{UserID=11,RoleID=6},
                new UserRole{UserID=12,RoleID=6},
                new UserRole{UserID=1,RoleID=5},
                new UserRole{UserID=2,RoleID=4},
                new UserRole{UserID=6,RoleID=4}
            };
            foreach (UserRole ur in userRoles)
            {
                context.UserRoles.Add(ur);
            }
            context.SaveChanges();

            //UserDomain
            var userDomains = new UserDomain[]
            {
                new UserDomain{UserID=1,DomainID=1,CompetitionCoef=0.83m},
                new UserDomain{UserID=2,DomainID=1,CompetitionCoef=0.68m},
                new UserDomain{UserID=3,DomainID=1,CompetitionCoef=0.76m},
                new UserDomain{UserID=4,DomainID=1,CompetitionCoef=0.67m},
                new UserDomain{UserID=5,DomainID=1,CompetitionCoef=0.71m},
                new UserDomain{UserID=6,DomainID=1,CompetitionCoef=0.45m},
                new UserDomain{UserID=7,DomainID=1,CompetitionCoef=0.58m},
                new UserDomain{UserID=8,DomainID=1,CompetitionCoef=0.64m},
                new UserDomain{UserID=9,DomainID=1,CompetitionCoef=0.42m},
                new UserDomain{UserID=10,DomainID=1,CompetitionCoef=0.92m},
                new UserDomain{UserID=11,DomainID=1,CompetitionCoef=0.87m},
                new UserDomain{UserID=12,DomainID=1,CompetitionCoef=0.79m},
                new UserDomain{UserID=3,DomainID=3,CompetitionCoef=0.79m},
                new UserDomain{UserID=4,DomainID=3,CompetitionCoef=0.81m},
                new UserDomain{UserID=5,DomainID=3,CompetitionCoef=0.92m},
            };
            foreach (UserDomain ud in userDomains)
            {
                context.UserDomains.Add(ud);
            }
            context.SaveChanges();
        }
    }
}
