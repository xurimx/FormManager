using FluentAssertions;
using FormManager.Application.Common.Models;
using FormManager.Application.Forms.Queries;
using FormManager.Application.IntegrationTests;
using FormManager.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.IntegrationTests.Forms.Queries
{
    using static Testing;
    public class GetFormsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnList()
        {
            await AddAsync(new Form
            {
                Appointment = DateTime.UtcNow.AddMinutes(80),
                Company = "Google",
                Date = DateTime.UtcNow,
                Email = "urim@morina.com",
                Id = Guid.NewGuid(),
                Name = "Urim",
                SenderId = Guid.NewGuid().ToString(),
                Telephone = "+383491234560"
            });

            var query = new GetFormsQuery();

            Pagination<Form> result = await SendAsync(query);


            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
        }
    }
}
