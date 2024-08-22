using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModernizationDemo.EfCoreTests.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // optional
            //migrationBuilder.DropTable(
            //    name: "__MigrationHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
