using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ModernizationDemo.LinqToSql
{
    public static class DatabaseHelper
    {
        public static void DropDatabase(string connectionString)
        {
            // connect to the master database
            var masterConnectionString = BuildMasterConnectionString(connectionString, out var testDatabaseName);
            using var tempConnection = new SqlConnection(masterConnectionString);
            tempConnection.Open();

            // drop and recreate the database
            var command = new SqlCommand(
                $"""
                 IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'{testDatabaseName}') BEGIN
                   ALTER DATABASE {testDatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                   DROP DATABASE {testDatabaseName}
                 END
                 """, tempConnection);
            command.ExecuteNonQuery();

            tempConnection.Close();
        }

        private static string BuildMasterConnectionString(string connectionString, out string originalDatabaseName)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            originalDatabaseName = builder.InitialCatalog;

            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        public static void CreateAndSeedDatabase(string connectionString)
        {
            // seed data
            using (var context = new DotNetCollegeContextDataContext(connectionString))
            {
                context.CreateDatabase();

                var suppliers = new List<Supplier>()
                {
                    new Supplier()
                    {
                        Name = "Supplier1",
                        Address = new Address()
                        {
                            City = "City1", 
                            Street = "Street1"
                        }
                    },
                    new Supplier()
                    {
                        Name = "Supplier2",
                        Address = new Address()
                        {
                            City = "City2",
                            Street = "Street2"
                        }
                    }
                };
                context.Suppliers.InsertAllOnSubmit(suppliers);

                var locations = new List<Location>()
                {
                    new Location()
                    {
                        Name = "Location1",
                        Address = "Address1"
                    },
                    new Location()
                    {
                        Name = "Location2",
                        Address = "Address2"
                    },
                    new Location()
                    {
                        Name = "Location3",
                        Address = "Address3"
                    }
                };
                context.Locations.InsertAllOnSubmit(locations);

                var mainCategories = new List<MainCategory>()
                {
                    new MainCategory()
                    {
                        Name = "MainCategory1",
                        Categories = 
                        {
                            new Category() { Name = "Category1" },
                            new Category() { Name = "Category2" }
                        }
                    },
                    new MainCategory()
                    {
                        Name = "MainCategory2",
                        Categories = 
                        {
                            new Category() { Name = "Category3" },
                            new Category() { Name = "Category4" },
                            new Category() { Name = "Category5" },
                            new Category() { Name = "Category6" },
                        }
                    }
                };
                context.MainCategories.InsertAllOnSubmit(mainCategories);

                var courseTemplates = new List<CourseTemplate>()
                {
                    new CourseTemplate()
                    {
                        Name = "CourseTemplate1",
                        Courses =
                        {
                            new Course()
                            {
                                BeginDate = new DateTime(2021, 10, 1, 9, 0, 0),
                                IsApproved = true,
                                AllowCashPayments = false,
                                AllowOnlinePayments = true,
                                Price = 12000,
                                Supplier = suppliers[0],
                                Location = locations[0]
                            },
                            new Course()
                            {
                                BeginDate = new DateTime(2023, 10, 17, 9, 0, 0),
                                IsApproved = true,
                                AllowCashPayments = false,
                                AllowOnlinePayments = true,
                                Price = 11000,
                                Supplier = suppliers[1],
                                Location = locations[2]
                            },
                            new Course()
                            {
                                BeginDate = new DateTime(2023, 12, 4, 9, 0, 0),
                                IsApproved = true,
                                AllowCashPayments = false,
                                AllowOnlinePayments = true,
                                Price = 9000,
                                Supplier = suppliers[0],
                                Location = locations[1]
                            }
                        },
                        CategoryCourseTemplates =
                        {
                            new CategoryCourseTemplate() { Category = mainCategories[0].Categories[0] }
                        }
                    },
                    new CourseTemplate()
                    {
                        Name = "CourseTemplate2",
                        Courses =
                        {
                            new Course()
                            {
                                BeginDate = new DateTime(2022, 4, 5, 9, 0, 0),
                                IsApproved = true,
                                AllowCashPayments = false,
                                AllowOnlinePayments = true,
                                Price = 13000,
                                Supplier = suppliers[1],
                                Location = locations[1]
                            },
                            new Course()
                            {
                                BeginDate = new DateTime(2024, 1, 22, 9, 0, 0),
                                IsApproved = true,
                                AllowCashPayments = false,
                                AllowOnlinePayments = true,
                                Price = 15000,
                                Supplier = suppliers[1],
                                Location = locations[2]
                            }
                        },
                        CategoryCourseTemplates =
                        {
                            new CategoryCourseTemplate() { Category = mainCategories[1].Categories[1] }
                        }
                    }
                };
                context.CourseTemplates.InsertAllOnSubmit(courseTemplates);

                context.SubmitChanges();
            }
        }
    }
}