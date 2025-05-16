---
id: overview
<<<<<<<< HEAD:src/clients/tfst-dev/docs/openiddict/architecture/overview.md
title: Overview
========
title: Architecture
>>>>>>>> fix/clean-merge-main:src/clients/tfst-dev/docs/openiddict/architecture/index.md
---

# Architecture Overview

This document provides an overview of the architecture of TFST.AuthServer, TFST.API, and tfst-demo projects, detailing how they interact with each other.

## TFST.AuthServer

TFST.AuthServer is the central authentication component that uses OpenIddict to manage user authentication. It provides a secure workflow for token issuance and credential validation.

## TFST.API

TFST.API acts as the interface between clients and the authentication server. It exposes various endpoints that allow clients to interact with protected resources, using access tokens issued by the AuthServer.

## tfst-demo

The tfst-demo client is a sample application that demonstrates how to interact with TFST.API. It uses the authentication flow provided by TFST.AuthServer to obtain tokens and access resources.

## Component Interaction

1. **Authentication**: Users authenticate through TFST.AuthServer, which validates credentials and issues an access token.
2. **API Access**: The tfst-demo client uses the access token to make requests to TFST.API.
3. **Resource Protection**: TFST.API validates the access token on each request to ensure only authenticated users can access resources.

This modular architecture allows for easy scalability and maintenance, ensuring that each component can evolve independently while maintaining system integrity as a whole.