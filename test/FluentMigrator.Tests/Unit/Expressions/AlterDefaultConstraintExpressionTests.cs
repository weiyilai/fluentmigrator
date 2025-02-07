#region License
//
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Tests.Helpers;

using NUnit.Framework;

using Shouldly;

namespace FluentMigrator.Tests.Unit.Expressions
{
    [TestFixture]
    [Category("Expression")]
    [Category("AlterDefaultConstraint")]
    public class AlterDefaultConstraintExpressionTests
    {
        [Test]
        public void ErrorIsReturnedWhenTableNameIsNull()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = null, ColumnName = "Column1", DefaultValue = SystemMethods.CurrentDateTime };

            var errors = ValidationHelper.CollectErrors(expression);
            errors.ShouldContain(ErrorMessages.TableNameCannotBeNullOrEmpty);
        }

        [Test]
        public void ErrorIsReturnedWhenColumnNameIsNull()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = "test", ColumnName = "", DefaultValue = SystemMethods.CurrentDateTime };

            var errors = ValidationHelper.CollectErrors(expression);
            errors.ShouldContain(ErrorMessages.ColumnNameCannotBeNullOrEmpty);
        }

        [Test]
        public void ErrorIsReturnedWhenDefaultValueIsNull()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = "test", ColumnName = "Column1", DefaultValue = null};

            var errors = ValidationHelper.CollectErrors(expression);
            errors.ShouldContain(ErrorMessages.DefaultValueCannotBeNull);
        }

        [Test]
        public void ErrorIsNotReturnedWhenTableNameAndColumnNameAndDefaultValueAreSet()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = "test", ColumnName = "Column1", DefaultValue = SystemMethods.CurrentDateTime};

            var errors = ValidationHelper.CollectErrors(expression);
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void WhenDefaultSchemaConventionIsAppliedAndSchemaIsNotSetThenSchemaShouldBeNull()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = "test", ColumnName = "Column1", DefaultValue = SystemMethods.CurrentDateTime };

            var processed = expression.Apply(ConventionSets.NoSchemaName);

            Assert.That(processed.SchemaName, Is.Null);
        }

        [Test]
        public void WhenDefaultSchemaConventionIsAppliedAndSchemaIsSetThenSchemaShouldNotBeChanged()
        {
            var expression = new AlterDefaultConstraintExpression { SchemaName = "testschema", TableName = "test", ColumnName = "Column1", DefaultValue = SystemMethods.CurrentDateTime };

            var processed = expression.Apply(ConventionSets.WithSchemaName);

            Assert.That(processed.SchemaName, Is.EqualTo("testschema"));
        }

        [Test]
        public void WhenDefaultSchemaConventionIsChangedAndSchemaIsNotSetThenSetSchema()
        {
            var expression = new AlterDefaultConstraintExpression { TableName = "test", ColumnName = "Column1", DefaultValue = SystemMethods.CurrentDateTime };

            var processed = expression.Apply(ConventionSets.WithSchemaName);

            Assert.That(processed.SchemaName, Is.EqualTo("testdefault"));
        }
    }
}
