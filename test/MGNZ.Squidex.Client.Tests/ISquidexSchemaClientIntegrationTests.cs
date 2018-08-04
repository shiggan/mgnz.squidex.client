namespace MGNZ.Squidex.Client.Tests
{
  using System;
  using System.Threading.Tasks;

  using FluentAssertions;

  using MGNZ.Squidex.Client.Tests.Stories;

  using Xunit;

  public class ISquidexSchemaClientIntegrationTests : SquidexClientIntegrationTest
  {
    [Fact]
    public async Task EndToEnd_HappyPath()
    {
      var oauthStories = new OAuthStories(this.Options);
      var schemaStories = new SchemaStories(this.Options);
      var knownUser = this.Options.Clients["aut-developer"];

      var oauthToken =
        await oauthStories.GetOAuthToken(knownUser.OAuthAppName, knownUser.OAuthClientId, knownUser.OAuthClientSecret);

      // todo : create app for the test; untill then verify we dont have any unexpected schemas on the app

      {
        // predcondition
        var allschemas = await schemaStories.GetSchemas("aut");
        int count = Convert.ToInt32(allschemas.Count);
        count.Should().Be(0, "Test cannot start because there are unexpected Schemas on the target endpoint");
      }

      // setup
      await schemaStories.PostSchema("aut", this.Schema1Asset.Value, "schema1name");
      await schemaStories.PostSchema("aut", this.Schema1Asset.Value, "schema2name");

      {
        var allschemas = await schemaStories.GetSchemas("aut");
        int count = Convert.ToInt32(allschemas.Count);
        count.Should().Be(2);
        string schema1name = Convert.ToString(allschemas[0].name);
        string schema2name = Convert.ToString(allschemas[1].name);
        schema1name.Should().NotBeNullOrWhiteSpace().And.NotBeEmpty();
        schema2name.Should().NotBeNullOrWhiteSpace().And.NotBeEmpty();
      }

      {
        await schemaStories.PublishSchema("aut", "schema1name");
        var schema1 = await schemaStories.GetSchema("aut", "schema1name");
        string schema1name = Convert.ToString(schema1.name);
        bool schema1published = Convert.ToBoolean(schema1.isPublished);
        schema1name.Should().Be("schema1name");
        schema1published.Should().BeTrue();
      }

      {
        await schemaStories.UnpublishSchema("aut", "schema1name");
        var schema1 = await schemaStories.GetSchema("aut", "schema1name");
        string schema1name = Convert.ToString(schema1.name);
        bool schema1published = Convert.ToBoolean(schema1.isPublished);
        schema1name.Should().Be("schema1name");
        schema1published.Should().BeFalse();
      }

      {
        await schemaStories.DeleteSchema("aut", "schema1name");
        await schemaStories.DeleteSchema("aut", "schema2name");
        var allschemas = await schemaStories.GetSchemas("aut");
        int count = Convert.ToInt32(allschemas.Count);
        count.Should().Be(0);
      }

      //// tidy
      //{
      //  await stories.DeleteSchema("aut", "schema2name");
      //  var allschemas = await stories.GetSchemas("aut");
      //  int count = Convert.ToInt32(allschemas.Count);
      //  count.Should().Be(0);
      //}

      // todo : delete app used for the test
    }
  }
}