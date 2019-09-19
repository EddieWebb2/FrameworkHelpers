using System;
using FrameworkHelpers.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace FrameworkHelpers.Tests.Models
{
    [TestClass]
    public class SystemTimeTests
    {
        [TestMethod]
        public void When_overriding_now_it_should_return_overriden_value()
        {
            DateTime fix = new DateTime(2019, 9, 19, 1, 1, 1);

            SystemTime.OverrideWith(() => fix);

            SystemTime.Now.ShouldBe(fix);
        }
    }
}