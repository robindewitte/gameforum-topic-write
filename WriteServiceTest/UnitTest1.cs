using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Xunit;
using writeservice;
using fictivusforum_writeservice.Repositories;
using fictivusforum_writeservice.DataModels;
using writeservice.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace WriteServiceTest
{
    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public Microsoft.Extensions.Configuration.IConfigurationRoot Configuration { get; private set; }

        public UnitTest1()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>().ConfigureAppConfiguration(config =>
               {
                   Configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json")
                     .Build();

                   config.AddConfiguration(Configuration);
               }));
            _client = _server.CreateClient();
        }

        [Fact]
        public void PostTopic_Preset_True()
        {
            //arrange
            var options = new DbContextOptionsBuilder<TopicContext>()
           .UseInMemoryDatabase(databaseName: "TopicDatabasetest1")
           .Options;


            DateTime generate = new DateTime(1999, 6, 24, 14, 45, 30);
            List<Topic> compare = new List<Topic>();
            compare.Add(new Topic { Username = "testboy1", TimeOfPosting = generate, Subject = "meme", Title = "dubbel" });
            compare.Add(new Topic { Username = "testboy1", TimeOfPosting = generate, Subject = "meme", Title = "enkel" });


            using (var context = new TopicContext(options))
            {
                context.Topics.Add(new Topic { Username = "testboy1", TimeOfPosting = generate, Subject = "meme", Title = "dubbel" });
                context.SaveChanges();
            }

            //act
            using (var context = new TopicContext(options))
            {
                WriteController controller = new WriteController(context);
                var boolone = controller.PostTopic("testboy1", "dubbel", generate, "meme");
                var booltwo = controller.PostTopic("testboy1", "enkel", generate, "meme");

                //assert
                Assert.False(boolone.Result);
                Assert.True(booltwo.Result);
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void PostResponse_Preset_True()
        {
            //arrange
            var options = new DbContextOptionsBuilder<TopicContext>()
           .UseInMemoryDatabase(databaseName: "TopicDatabasetest2")
           .Options;


            DateTime generate = new DateTime(1999, 6, 24, 14, 45, 30);
            List<Response> compare = new List<Response>();
            compare.Add(new Response { UserName= "testboy1", TopicTitle = "dubbel", TimeOfPosting = generate, TopicSubject = "meme", Content = "dubbel" });
            compare.Add(new Response { UserName = "testboy1", TopicTitle = "enkel", TimeOfPosting = generate, TopicSubject = "meme", Content = "enkel" });


            using (var context = new TopicContext(options))
            {
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "dubbel", TimeOfPosting = generate, TopicSubject = "meme", Content = "dubbel" });
                context.SaveChanges();
            }

            //act
            using (var context = new TopicContext(options))
            {
                WriteController controller = new WriteController(context);
                var boolone = controller.PostResponse("dubbel", "testboy1",generate,  "dubbel", "meme");
                var booltwo = controller.PostResponse("enkel", "testboy1", generate, "enkel", "meme");

                //assert
                Assert.False(boolone.Result);
                Assert.True(booltwo.Result);
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void DeleteResponse_Preset_True()
        {
            //arrange
            var options = new DbContextOptionsBuilder<TopicContext>()
           .UseInMemoryDatabase(databaseName: "TopicDatabasetest3")
           .Options;

            DateTime generate = new DateTime(1999, 6, 24, 14, 45, 30);

            using (var context = new TopicContext(options))
            {
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "dubbel", TimeOfPosting = generate, TopicSubject = "meme", Content = "dubbel" });
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "enkel", TimeOfPosting = generate, TopicSubject = "meme", Content = "enkel" });
                context.SaveChanges();
            }

            //act
            using (var context = new TopicContext(options))
            {
                WriteController controller = new WriteController(context);
                var boolone = controller.DeleteResponse("dubbel", "testboy1", generate, "dubbel");


                //assert
                Assert.True(boolone.Result);
                Assert.Equal(0, context.Responses.Where(b => b.UserName == "testboy1" && b.TopicTitle == "dubbel" && b.TimeOfPosting == generate && b.TopicSubject == "meme" && b.Content == "dubbel").Count());
                Assert.Equal(1, context.Responses.Where(b => b.UserName == "testboy1" && b.TopicTitle == "enkel" && b.TimeOfPosting == generate && b.TopicSubject == "meme" && b.Content == "enkel").Count());
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void DeleteTopic_Preset_True()
        {
            //arrange
            var options = new DbContextOptionsBuilder<TopicContext>()
           .UseInMemoryDatabase(databaseName: "TopicDatabasetest4")
           .Options;


            DateTime generate = new DateTime(1999, 6, 24, 14, 45, 30);
          
            //deze nog afmaken

            using (var context = new TopicContext(options))
            {
                context.Topics.Add(new Topic { Username = "testboy1", Title = "dubbel", TimeOfPosting = generate, Subject = "meme" });
                context.Topics.Add(new Topic { Username = "testboy1", Title = "enkel", TimeOfPosting = generate, Subject = "meme" });
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "dubbel", TimeOfPosting = generate, TopicSubject = "meme", Content = "dubbel" });
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "dubbel", TimeOfPosting = generate, TopicSubject = "meme", Content = "dubbel2" });
                context.Responses.Add(new Response { UserName = "testboy1", TopicTitle = "enkel", TimeOfPosting = generate, TopicSubject = "meme", Content = "enkel" });
                context.SaveChanges();
            }

            //act
            using (var context = new TopicContext(options))
            {
                WriteController controller = new WriteController(context);
                var boolone = controller.DeleteTopic("dubbel");


                //assert
                Assert.True(boolone.Result);
                Assert.Equal(0, context.Responses.Where(b => b.UserName == "testboy1" && b.TopicTitle == "dubbel" && b.TimeOfPosting == generate && b.TopicSubject == "meme" && b.Content == "dubbel").Count());
                Assert.Equal(0, context.Topics.Where(b => b.Username == "testboy1" && b.Title == "dubbel" && b.TimeOfPosting == generate && b.Subject == "meme").Count());
                Assert.Equal(1, context.Responses.Where(b => b.UserName == "testboy1" && b.TopicTitle == "enkel" && b.TimeOfPosting == generate && b.TopicSubject == "meme" && b.Content == "enkel").Count());
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}
