# Dapr Demo

[![Build Status](https://dev.azure.com/headleysj/Demos/_apis/build/status%2Fdapr-demo?branchName=main)](https://dev.azure.com/headleysj/Demos/_build/latest?definitionId=17&branchName=main)

This is a basic demo application that processes orders within a system, leveraging Dapr components to do various things easily.

## Components used

* Virtual Actors
* Pub/Sub
* Secret Management
* Service Invocation

## Repository structure

This repository is also a learning space for me to get comfortable implementing [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture).

## Tests

### Postman

You can import the Postman collection and environment from the `test/postman-tests` folder in the repository.

### Strategy

This project follows a very similar testing strategy as [Microsoft's DevOps test taxonomy](https://learn.microsoft.com/en-us/devops/develop/shift-left-make-testing-fast-reliable#devops-test-taxonomy) with tests ranging from L0 -> L4.

### Syntax

Tests are written using [Gherkin syntax](https://cucumber.io/docs/gherkin/reference/).

### Test Framework

Tests are written using:

* NUnit
* NSubstitute
* FluentAssertions
