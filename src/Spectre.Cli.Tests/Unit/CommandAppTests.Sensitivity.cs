using Shouldly;
using Spectre.Cli.Exceptions;
using Spectre.Cli.Testing;
using Spectre.Cli.Testing.Data.Commands;
using Spectre.Cli.Testing.Data.Settings;
using Xunit;

namespace Spectre.Cli.Tests.Unit
{
    public sealed partial class CommandApptests
    {
        [Fact]
        public void Should_Treat_Commands_As_Case_Sensitive_If_Specified()
        {
            // Given
            var app = new CommandAppFixture();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.Commands);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "Command", "--foo", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown command 'Command'.");
            });
        }

        [Fact]
        public void Should_Treat_Long_Options_As_Case_Sensitive_If_Specified()
        {
            // Given
            var app = new CommandAppFixture();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.LongOptions);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "command", "--Foo", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown option 'Foo'.");
            });
        }

        [Fact]
        public void Should_Treat_Short_Options_As_Case_Sensitive()
        {
            // Given
            var app = new CommandAppFixture();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "command", "-F", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown option 'F'.");
            });
        }

        [Fact]
        public void Should_Suppress_Case_Sensitivity_If_Specified()
        {
            // Given
            var app = new CommandAppFixture();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.None);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var (result, _, _, settings) = app.Run(new[]
            {
                "Command", "--Foo", "bar",
            });

            // Then
            result.ShouldBe(0);
            settings.ShouldBeOfType<StringOptionSettings>().And(vec =>
            {
                vec.Foo.ShouldBe("bar");
            });
        }
    }
}
