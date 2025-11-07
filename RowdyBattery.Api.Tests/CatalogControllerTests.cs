using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;
using RowdyBattery.Api.Controllers;
using RowdyBattery.Api.DTOs;
using RowdyBattery.Domain.Catalog;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace RowdyBattery.Api.Tests
{
    public class CatalogControllerTests
    {
        [Fact]
        public async Task AddRating_HappyPath_AddsRating()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase("AddRating_HappyPath")
                .Options;

            using var context = new StoreContext(options);
            var item = new Item(1, "Test Item", 10m);
            context.Items.Add(item);
            await context.SaveChangesAsync();

            var controller = new CatalogController(context);
            var result = await controller.AddRating(item.Id, new CreateRatingDto { Stars = 5, UserName = "tester", Review = "Nice" });

            Assert.IsType<OkResult>(result);

            var dbItem = await context.Items.Include(i => i.Ratings).FirstAsync(i => i.Id == item.Id);
            Assert.Single(dbItem.Ratings);
            Assert.Equal(5, dbItem.Ratings.First().Stars);
            Assert.Equal("tester", dbItem.Ratings.First().UserName);
        }

        [Fact]
        public async Task AddRating_InvalidStars_ReturnsBadRequest()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase("AddRating_InvalidStars")
                .Options;

            using var context = new StoreContext(options);
            var item = new Item(1, "Test Item", 10m);
            context.Items.Add(item);
            await context.SaveChangesAsync();

            var controller = new CatalogController(context);
            var result = await controller.AddRating(item.Id, new CreateRatingDto { Stars = 0, UserName = "tester" });

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
