using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.DAL
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SubsystemContext>
    {
        protected override void Seed(SubsystemContext context)
        {
            //Пользователи
            var users = new List<User>
            {
                new User{Name="Alyana",LastName="Matyusheva",Email="alk-fifa@yandex.ru", Password="123"},
                new User{Name="Grigorii",LastName="Praskura",Email="gregorypraskura@hotmail.com", Password="12345"},
                new User{Name="Ivan",LastName="Ivanov",Email="ivanov@ya.ru", Password="123"},
                new User{Name="Petr",LastName="Petrov",Email="petrov@yandex.ru", Password="123"},
                new User{Name="Sergey",LastName="Sidorov",Email="sidorov@yandex.ru", Password="123"}
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
            
            // Роли
            var roles = new List<Role>
            {
                new Role{Name="ЛПР", Description=""},
                new Role{Name="Главный менеджер", Description=""},
                new Role{Name="Координатор", Description=""},
                new Role{Name="Аналитик", Description=""},
                new Role{Name="Куратор экспертного сообщества", Description=""},
                new Role{Name="Эксперт", Description=""}
            };
            roles.ForEach(r => context.Roles.Add(r));
            context.SaveChanges();
        }
    }
}