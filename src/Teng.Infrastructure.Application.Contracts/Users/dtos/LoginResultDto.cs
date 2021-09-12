﻿namespace Teng.Infrastructure.Users.dtos
{
    public class LoginResultDto
    {
        /// <summary>Gets the access token.</summary>
        /// <value>The access token.</value>
        public string AccessToken { get; set; }

        /// <summary>Gets the identity token.</summary>
        /// <value>The identity token.</value>
        public string IdentityToken { get; set; }

        /// <summary>Gets the type of the token.</summary>
        /// <value>The type of the token.</value>
        public string TokenType { get; set; }

        /// <summary>Gets the refresh token.</summary>
        /// <value>The refresh token.</value>
        public string RefreshToken { get; set; }

        /// <summary>Gets the error description.</summary>
        /// <value>The error description.</value>
        public string ErrorDescription { get; set; }

        /// <summary>Gets the expires in.</summary>
        /// <value>The expires in.</value>
        public int ExpiresIn { get; set; }
    }
}