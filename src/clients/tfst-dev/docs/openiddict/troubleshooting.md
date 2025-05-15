---
id: troubleshooting
title: Troubleshooting
---

# Troubleshooting Guide

## Common Issues

### Issue 1: Authentication Failure
- **Description**: Users cannot authenticate.
- **Solution**: Ensure that AuthServer is running and that the correct client credentials are being used. Check logs for error messages.

### Issue 2: API Not Responding
- **Description**: The API is not responding to requests.
- **Solution**: Verify that the API service is up and running. Check network configurations and ensure the API endpoint is correctly specified.

### Issue 3: OpenIddict Configuration Errors
- **Description**: Errors related to OpenIddict configuration.
- **Solution**: Review OpenIddict configuration in configuration files. Make sure all required parameters are properly configured.

## Debugging Tips
- Use logging to capture detailed information about application behavior.
- Review console output for errors or warnings during runtime.
- Check documentation to verify any configuration that might have been overlooked.

## Additional Resources
- [OpenIddict Documentation](https://documentation.openiddict.com/)
- [API Documentation](./architecture/api.md)

This guide aims to help users resolve common issues encountered while using TFST.AuthServer, TFST.API, and tfst-demo projects.