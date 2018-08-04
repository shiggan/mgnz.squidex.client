namespace MGNZ.Squidex.Client
{
  using System.Threading.Tasks;

  using MGNZ.Squidex.Client.Model;
  using MGNZ.Squidex.Client.Transport;

  using Refit;

  [Headers("Cache-Control: no-cache", "Connection: keep-alive", "Pragma: no-cache")]
  public interface ISquidexOAuthClient
  {
    [Post("/identity-server/connect/token")]
    Task<SquidexGetOAuthTokenResponse> GetToken([Body(BodySerializationMethod.UrlEncoded)]
      SquidexGetOAuthTokenRequest request);
  }
}