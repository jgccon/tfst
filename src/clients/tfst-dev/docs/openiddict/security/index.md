---
id: openiddict-overview
title: Overview
sidebar_position: 0
---

# OpenIddict Overview

OpenIddict is an open-source library that provides a simple way to implement an OpenID Connect server in your applications. It is designed to be easy to use and integrate with existing ASP.NET Core applications, allowing developers to secure their APIs and manage user authentication and authorization effectively.

## Purpose of OpenIddict

The main purpose of OpenIddict is to enable developers to create secure authentication flows for their applications. It supports various authentication methods, including password-based authentication, social login, and more. OpenIddict also enables the issuance of access tokens, refresh tokens, and ID tokens, which are essential for securing API endpoints.

## Integration with TFST.AuthServer

In the context of the TFST.AuthServer project, OpenIddict is used to manage user authentication and issue tokens needed to access protected resources in TFST.API projects through the tfst-demo client. The integration involves configuring OpenIddict services in the ASP.NET Core application, setting up the necessary database tables for token storage, and defining authentication flows.

## Key Features

- **Token Management**: OpenIddict handles token creation, validation, and revocation.
- **Customizable**: Developers can customize authentication flows and token issuance processes to meet their specific requirements.
- **Multiple Grant Type Support**: OpenIddict supports various OAuth2 grant types, including authorization code, client credentials, and resource owner password credentials.

## OpenIddict Website
For more information, documentation, and usage examples, visit the [official OpenIddict website](https://openiddict.com/).