using System;
using System.Net;
using System.Security;

using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using JsonFx.JsonRpc;

namespace OpenIDDialog.Services
{
	[JsonService(Namespace="OpenID", Name="Service")]
	public class OpenIDService
	{
		/// <summary>
		/// Stage 2: user submits Identifier
		/// </summary>
		/// <param name="openid_identifier"></param>
		/// <returns></returns>
		[JsonMethod(Name="beginAuth")]
		public string BeginAuthentication(string identifier, string returnUrl)
		{
			Identifier id;
			if (!Identifier.TryParse(identifier, out id))
			{
				throw new ArgumentException("Invalid identifier");
			}

			Uri url;
			if (!Uri.TryCreate(returnUrl, UriKind.RelativeOrAbsolute, out url))
			{
				throw new ArgumentException("Invalid returnUrl");
			}

			Realm realm = new Realm(new Uri(url, "/"));

			OpenIdRelyingParty openid = new OpenIdRelyingParty();
			IAuthenticationRequest request = openid.CreateRequest(identifier, realm, url);
			return request.RedirectingResponse.Headers[HttpResponseHeader.Location];
		}

		/// <summary>
		/// Stage 3: OpenID Provider sends assertion response
		/// </summary>
		/// <returns></returns>
		//[JsonMethod(Name="endAuth")]
		public Identifier EndAuthentication(out string friendlyIdentifier)
		{
			OpenIdRelyingParty openid = new OpenIdRelyingParty();
			IAuthenticationResponse response = openid.GetResponse();
			if (response == null)
			{
				friendlyIdentifier = null;
				return null;
			}
			if (response.Status != AuthenticationStatus.Authenticated)
			{
				string message;
				if (response.Exception == null)
				{
					message = response.Status.ToString();
				}
				else
				{
					message = response.Exception.Message;
				}

				throw new SecurityException(message, response.Exception);
			}

			friendlyIdentifier = response.FriendlyIdentifierForDisplay;
			return response.ClaimedIdentifier;
		}
	}
}