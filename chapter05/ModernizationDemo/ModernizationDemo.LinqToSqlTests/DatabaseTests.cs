using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace ModernizationDemo.LinqToSql
{
    [TestClass]
    public class DatabaseTests
    {
        private const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=LinqToSqlTest;Integrated Security=True;TrustServerCertificate=True";

        private DotNetCollegeContextDataContext context;

        [TestInitialize]
        public void TestInit()
        {
            DatabaseHelper.DropDatabase(connectionString);
            DatabaseHelper.CreateAndSeedDatabase(connectionString);

            context = new DotNetCollegeContextDataContext(connectionString);
            context.Log = Console.Out;
        }

        [TestMethod]
        public void LazyLoading()
        {
            // load recent courses
            var courses = context.Courses
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.BeginDate)
                .Take(20)
                .ToList();
            Assert.AreEqual(5, courses.Count);

            // use lazy-loading to access related data
            Assert.AreEqual("CourseTemplate2", courses[0].CourseTemplate.Name);
        }

        [TestMethod]
        public void EagerLoading()
        {
            // use eager-loading to load related data
            var loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<MainCategory>(c => c.Categories);
            context.LoadOptions = loadOptions;

            // load categories with eager loaded data
            var mainCategories = context.MainCategories.ToList();

            Assert.AreEqual(2, mainCategories.Count);
            Assert.AreEqual(2, mainCategories[0].Categories.Count);
            Assert.AreEqual(4, mainCategories[1].Categories.Count);
        }

        [TestMethod]
        public void Projection()
        {
            // projection to avoid eager loading
            var categoryTree = context.MainCategories
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Categories = c.Categories
                        .Select(sc => new
                        {
                            Id = sc.Id,
                            Name = sc.Name
                        })
                })
                .ToList();

            Assert.AreEqual(2, categoryTree.Count);
            Assert.AreEqual(2, categoryTree[0].Categories.Count());
            Assert.AreEqual(4, categoryTree[1].Categories.Count());

        }

        [TestMethod]
        public void InsertUpdateDelete()
        {
            var courses = context.Courses.ToList();

            // update a course
            courses[0].Price = 10000;
            courses[0].IsApproved = true;

            // delete a course
            context.Courses.DeleteOnSubmit(courses[1]);

            // add a new course (with related entities)
            var newCourse = new Course()
            {
                IsApproved = true,
                AllowCashPayments = false,
                AllowOnlinePayments = true,
                CourseTemplateId = 1,
                Price = 12345,
                SupplierId = 1,
                LocationId = 1
            };
            newCourse.CourseDates.Add(new CourseDate()
            {
                BeginDate = new DateTime(2024, 8, 1, 9, 0, 0),
                EndDate = new DateTime(2024, 8, 1, 17, 0, 0)
            });
            newCourse.CourseDates.Add(new CourseDate()
            {
                BeginDate = new DateTime(2024, 8, 2, 9, 0, 0),
                EndDate = new DateTime(2024, 8, 2, 17, 0, 0)
            });
            context.Courses.InsertOnSubmit(newCourse);

            // save changes
            context.SubmitChanges();

            // ensure the course was added
            Assert.AreEqual(1, context.ExecuteQuery<int>("SELECT COUNT(*) FROM Courses WHERE Price = 12345").First());
            Assert.AreEqual(2, context.ExecuteQuery<int>("SELECT COUNT(*) FROM CourseDates WHERE CourseId = 6").First());

            // ensure the course was updated
            Assert.AreEqual(1, context.ExecuteQuery<int>("SELECT COUNT(*) FROM Courses WHERE Id = 1 AND IsApproved = 1 AND Price = 10000").First());

            // ensure the course was deleted
            Assert.AreEqual(0, context.ExecuteQuery<int>("SELECT COUNT(*) FROM Courses WHERE Id = 2").First());
        }


        [TestCleanup]
        public void TestCleanup()
        {
            context?.Dispose();
        }
    }
}
