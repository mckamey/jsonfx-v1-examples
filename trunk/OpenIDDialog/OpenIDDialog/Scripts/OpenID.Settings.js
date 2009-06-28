/*global OpenID */

/* namespace OpenID */
if ("undefined" === typeof window.OpenID) {
	window.OpenID = {};
}

/* Settings */
OpenID.Settings = {
	
	/* default strings */
	providerLabel : "Enter your {provider} {username}",
	userLabel : "username",
	promptLabel : "Click a button to sign in to your provider.",
	signInLabel : "Continue",

	/* image location */
	spritePath : "",

	/* Providers
		name: the provider name
		url: a format string for generating the OpenID URL
		userLabel: the label provider uses for username
		big: means that icon will be big
	*/
	providers : [
		{
			name: "Google",
			label: "Continue to enter your {provider} {username}",
			url: "https://www.google.com/accounts/o8/id",
			big: true
		},
		{
			name: "Yahoo",
			label: "Continue to enter your {provider} {username}",
			url: "http://yahoo.com/",
			big: true
		},
		{
			name: "Flickr",
			url: "http://flickr.com/{username}/",
			big: true
		},
		{
			name: "AOL",
			userLabel: "screenname",
			url: "http://openid.aol.com/{username}",
			big: true
		},
		{
			name: "OpenID",
			userLabel: "URL",
			url: "",
    		big: true
		},
		{
			name: "Blogger",
			userLabel: "account",
			url: "http://{username}.blogspot.com/"
		},
		{
			name: "WordPress",
			url: "http://{username}.wordpress.com/"
		},
		{
			name: "LiveJournal",
			url: "http://{username}.livejournal.com"
		},
		{
			name: "MySpace",
			url: "http://www.myspace.com/{username}"
		},
		{
			name: "Technorati",
			url: "http://technorati.com/people/technorati/{username}/"
		},
		{
			name: "VeriSign",
			url: "http://{username}.pip.verisignlabs.com/"
		},
		{
			name: "Vidoop",
			url: "http://{username}.myvidoop.com/"
		},
		{
			name: "ClaimID",
			url: "http://claimid.com/{username}"
		},
		{
			name: "myOpenID",
			url: "http://{username}.myopenid.com/"
		}
	]
};
