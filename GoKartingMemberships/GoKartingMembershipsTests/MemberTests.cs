using GoKartingMemberships;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoKartingMembershipsTests
{
    [TestFixture]
    public class MemberTests
    {
        private Mock<EmailSender> emailSenderMock;
        private Mock<FileLogger> mockFileLogger;

        private Member member;

        [SetUp]
        public void SetUp()
        {
            emailSenderMock = new Mock<EmailSender>();
            mockFileLogger = new Mock<FileLogger>();

            member = new Member(
                emailSenderMock.Object,
                mockFileLogger.Object
                );
        }

        [Test]
        public void SetRenewalDate_WhenValidTierIsProvided_ReturnsTrue()
        {
            // Arrange
            string validTier = "Gold";
            // Act
            bool result = member.setRenewalDate(validTier);
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SetRenewalDate_WhenInvalidTierIsProvided_ReturnsFalse()
        {
            // Arrange
            string invalidTier = "Platinum";
            // Act
            bool result = member.setRenewalDate(invalidTier);
            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SendRenewalDiscount_WhenEmailSentSuccessfully_ReturnsTrue()
        {
            // Arrange
            emailSenderMock.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(true);
            // Act
            bool result = member.sendRenewalDiscount();
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SendRenewalDiscount_WhenEmailSendingFails_ReturnsFalse()
        {
            // Arrange
            emailSenderMock.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Throws<Exception>();
            // Act
            bool result = member.sendRenewalDiscount();
            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CalculateDiscountPercentage_WhenMemberTypeIsGold_Returns24()
        {
            // Arrange
            member.setRenewalDate("Gold");
            // Act
            int result = member.calculateDiscountPercentage();
            // Assert
            Assert.AreEqual(24, result);
        }

        [Test]
        public void CalculateDiscountPercentage_WhenMemberTypeIsSilver_Returns12()
        {
            // Arrange
            member.setRenewalDate("Silver");
            // Act
            int result = member.calculateDiscountPercentage();
            // Assert
            Assert.AreEqual(12, result);
        }

        [Test]
        public void CalculateDiscountPercentage_WhenMemberTypeIsBronze_Returns6()
        {
            // Arrange
            member.setRenewalDate("Bronze");
            // Act
            int result = member.calculateDiscountPercentage();
            // Assert
            Assert.AreEqual(6, result);
        }

        [Test]
        public void CalculateDiscountPercentage_WhenMemberTypeIsNotSet_Returns0()
        {
            // Act
            int result = member.calculateDiscountPercentage();
            // Assert
            Assert.AreEqual(0, result);
        }
    }
}
