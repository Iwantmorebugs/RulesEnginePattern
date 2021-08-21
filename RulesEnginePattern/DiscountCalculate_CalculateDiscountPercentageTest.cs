﻿using System;
using FluentAssertions;
using Xunit;

namespace RulesEnginePattern
{
    public class DiscountCalculateCalculateDiscountPercentageTest
    {
        private const int DefaultAge = 30;
        private readonly DiscountCalculator _calculator = new();

        [Fact]
        public void Returns0PctForBasicCustomer()
        {
            var customer = CreateCustomer(DefaultAge, DateTime.Today.AddDays(-1));

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(0m);
        }

        [Fact]
        public void Returns5PctForCustomersOver65()
        {
            var customer = CreateCustomer(65, DateTime.Today.AddDays(-1));

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.05m);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(70)]
        public void Returns15PctForCustomerFirstPurchase(int customerAge)
        {
            var customer = CreateCustomer(customerAge);

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.15m);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(70)]
        public void Returns10PctForCustomersWhoAreVeterans(int customerAge)
        {
            var customer = CreateCustomer(customerAge, DateTime.Today.AddDays(-1));
            customer.IsVeteran = true;

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.10m);
        }

        [Theory]
        [InlineData(1, .05)]
        [InlineData(2, .08)]
        [InlineData(5, .10)]
        [InlineData(10, .12)]
        [InlineData(15, .15)]
        public void ReturnsCorrectLoyaltyDiscountForLongtimeCustomers(int yearsAsCustomer, decimal expectedDiscount)
        {
            var customer = CreateCustomer(DefaultAge,
                DateTime.Today.AddYears(-yearsAsCustomer).AddDays(-1));

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(expectedDiscount);
        }

        [Theory]
        [InlineData(1, .15)]
        [InlineData(2, .18)]
        [InlineData(5, .20)]
        [InlineData(10, .22)]
        [InlineData(15, .25)]
        public void ReturnsCorrectLoyaltyDiscountForLongtimeCustomersOnTheirBirthday(int yearsAsCustomer,
            decimal expectedDiscount)
        {
            var customer = CreateBirthdayCustomer(DefaultAge, DateTime.Today.AddYears(-yearsAsCustomer).AddDays(-1));

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(expectedDiscount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ReturnsVeteransDiscountForLoyal1And2YearCustomers(int yearsAsCustomer)
        {
            var customer = CreateCustomer(DefaultAge,
                DateTime.Today.AddYears(-yearsAsCustomer).AddDays(-1));
            customer.IsVeteran = true;

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.10m);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ReturnsVeteransDiscountForLoyal1And2YearCustomersOnBirthday(int yearsAsCustomer)
        {
            var customer = CreateBirthdayCustomer(DefaultAge,
                DateTime.Today.AddYears(-yearsAsCustomer).AddDays(-1));
            customer.IsVeteran = true;

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.20m);
        }

        [Fact]
        public void Returns10PctForCustomerSecondPurchaseOnBirthday()
        {
            var customer = CreateBirthdayCustomer(20, DateTime.Today.AddDays(-1));

            var result = _calculator.CalculateDiscountPercentage(customer);

            result.Should().Be(.10m);
        }

        private Customer CreateCustomer(int age = DefaultAge, DateTime? firstPurchaseDate = null)
        {
            return new()
            {
                DateOfBirth = DateTime.Today.AddYears(-age).AddDays(-1),
                DateOfFirstPurchase = firstPurchaseDate
            };
        }

        private Customer CreateBirthdayCustomer(int age = DefaultAge, DateTime? firstPurchaseDate = null)
        {
            return new()
            {
                DateOfBirth = DateTime.Today.AddYears(-age),
                DateOfFirstPurchase = firstPurchaseDate
            };
        }
    }
}