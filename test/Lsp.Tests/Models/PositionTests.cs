using FluentAssertions;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using TestingUtils;
using Xunit;

namespace Lsp.Tests.Models
{
    public class PositionTests
    {
        [Theory]
        [JsonFixture]
        public void SimpleTest(string expected)
        {
            var model = new Position(1, 1);
            var result = Fixture.SerializeObject(model);

            result.Should().Be(expected);

            var deresult = new LspSerializer(ClientVersion.Lsp3).DeserializeObject<Position>(expected);
            deresult.Should().BeEquivalentTo(model, x => x.UsingStructuralRecordEquality());
        }

        [Fact]
        public void Is_Sortable_By_Character()
        {
            var a = new Position(1, 1);
            var b = new Position(1, 2);
            var c = new Position(1, 1);

            a.Should().BeLessThan(b);
            b.Should().BeGreaterThan(a);

            a.CompareTo(c).Should().Be(0);

            a.Should().BeLessThanOrEqualTo(c);
            a.Should().BeGreaterThanOrEqualTo(c);
        }

        [Fact]
        public void Is_Sortable_By_Line()
        {
            var a = new Position(1, 1);
            var b = new Position(2, 1);
            var c = new Position(1, 1);

            a.Should().BeLessThan(b);
            b.Should().BeGreaterThan(a);

            a.CompareTo(c).Should().Be(0);

            a.Should().BeLessThanOrEqualTo(c);
            a.Should().BeGreaterThanOrEqualTo(c);
        }

        [Fact]
        public void Should_Support_Delta()
        {
            var a = new Position(1, 1);
            a = a.Delta(deltaLine: 1);
            a.Line.Should().Be(2);
            a = a.Delta(deltaCharacter: -1);
            a.Character.Should().Be(0);
            a = a.Delta(-1, 1);
            a.Should().Be((1, 1));
        }
    }
}
