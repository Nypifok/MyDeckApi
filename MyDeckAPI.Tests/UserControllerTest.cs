using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyDeckAPI.Controllers;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using NUnit.Framework;

namespace MyDeckAPI.Tests
{
    [TestFixture]
    class UserControllerTest
    {
        [Test]
        public void FindAllTest()
        {
            var mock = new Mock<IGenericRepository<User>>();


            var mock1 = new Mock<ILogger<UserController>>();
            UserController controller = new UserController(mock1.Object, mock.Object);
            Assert.AreEqual(result, controller.FindAll());

        }
        
       
        
    }
}
