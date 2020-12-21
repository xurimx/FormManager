using FluentAssertions;
using FluentValidation;
using FormManager.Application.Forms.Commands;
using FormManager.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.IntegrationTests.Forms.Commands
{
    using static Testing;
    public class CreateFormTests : TestBase
    {
        [Test]
        public void ShouldRequireAllFields()
        {
            var command = new CreateFormCommand
            {
                Name = "Urim"
            };

            FluentActions.Invoking(async () => {
                await SendAsync(command);
            }).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireFutureDateAsync()
        {
            var userId = await RunAsDefaultUserAsync();
            await SetupSmtp();

            var command = new CreateFormCommand
            {
                Name = "Urim",
                Company = "Google",
                Email = "urim@morina.com",
                Telephone = "+38349123456",
                Appointment = DateTime.UtcNow.AddDays(-1),
            };

            FluentActions.Invoking(async () => {
                await SendAsync(command);
            }).Should().Throw<ValidationException>();

            command.Appointment = DateTime.UtcNow.AddMinutes(80);

            var id = await SendAsync(command);
            id.Should<Guid>();
            id.Should().NotBe(Guid.Empty);

            Form form = await FindAsync<Form>(id);

            form.Should().NotBeNull();
            form.Name.Should().Be(command.Name);
            form.Telephone.Should().Be(command.Telephone);
            form.Appointment.Should().Be(command.Appointment);
            form.Email.Should().Be(command.Email);
            form.Date.Should().BeCloseTo(DateTime.Now, 10000);
        }

        [Test]
        public void ShouldRequireValidPhoneNumber()
        {

        }
    }
}
