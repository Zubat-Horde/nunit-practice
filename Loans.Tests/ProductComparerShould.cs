﻿using Loans.Domain.Applications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Tests
{
    [Category("Product Comparison")]
    public class ProductComparerShould
    {
        private List<LoanProduct> products;
        private ProductComparer sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // assuming that the list isnt changed by any of the tests
            products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3),
            };
        }

        [SetUp]
        public void Setup()
        {
            sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);
        }

        [TearDown]
        public void TearDown()
        {
            // Runs after each test executes
        }

        [Test]
        public void ReturnCorrectNumberOfComparisons()
        {
            List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons, Has.Exactly(3).Items);
        }

        [Test]
        public void NotReturnDuplicateComparisons()
        {
            List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons, Is.Unique);
        }

        [Test]
        public void ReturnComparisonForFirstProduct()
        {
            List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

            // Need to also know the expected monthly repayment
            var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);
            Assert.That(comparisons, Does.Contain(expectedProduct));
        }

        [Test]
        public void ReturnComparisonForFirstProduct_WithPartialKnownExpectedValues()
        {
            List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons, Has.Exactly(1)
                .Matches<MonthlyRepaymentComparison>(
                    item => item.ProductName == "a" &&
                            item.InterestRate == 1 &&
                            item.MonthlyRepayment > 0));
        }
    }
}
