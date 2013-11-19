Mongo2GoNUnit
=============

A tiny sample to see if Mongo2Go plays nice with NUnit.

I'm rather new to Mongo and trying to find a simple way to write integration tests that can be run via a familiar build chain (involving MSBuild and TeamCity).

Mongo2Go looks great but I'm having issues when running it via the Resharper test runner.

Current erroneous behaviour
--------------

1. First test run via Resharper passes.
2. Subsequent test runs fail with the error:
    TearDown : System.UnauthorizedAccessException : Access to the path 'IntegrationTest.0' is denied.
