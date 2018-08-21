// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  scope: "profile openid email offline_access role",
  grantType: "password",
  tokenEndPoint: "http://localhost:5000/connect/token",
  registerEndPoint: "http://localhost:5000/account/register",
  rootUrl: "http://localhost:5000"
};
