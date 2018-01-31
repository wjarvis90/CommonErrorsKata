﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CommonErrorsKata.Shared;

namespace CommonErrors.Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void ShouldOnlyAllowTenAnswers()
        {
            //Arrange
            var size = 10;
            var queue = new AnswerQueue<TrueFalseAnswer>(size);

            //Act
            for (int i = 0; i < size + 1; i++)
            {
                queue.Enqueue(new TrueFalseAnswer(true));
            }
            //Assert
            Assert.IsTrue(queue.Count <= 10);
        }

        [Test]
        public void ShouldForgetAtCapacity()
        {
            //Arrange
            var size = 10;
            var queue = new AnswerQueue<TrueFalseAnswer>(size);
            queue.Enqueue(new TrueFalseAnswer(false));
            for (var i = 0; i < 10; i++)
            {
                queue.Enqueue(new TrueFalseAnswer(true));
            }
            //Act
            var grade = queue.Grade;
            
            //Assert
            Assert.AreEqual(100, grade);
        }

        [Test]
        public void ShouldReturnExpectedAverage()
        {
            //Arrange
            var size = 10;
            var queue = new AnswerQueue<TrueFalseAnswer>(size);
            queue.Enqueue(new TrueFalseAnswer(false));
            queue.Enqueue(new TrueFalseAnswer(true));
            queue.Enqueue(new TrueFalseAnswer(true));
            queue.Enqueue(new TrueFalseAnswer(false));

            //Act
            var grade = queue.Grade;

            //Assert
            Assert.AreEqual(50, grade);
        }
    }
}
