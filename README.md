[![EasyDoubles NuGet Package](https://img.shields.io/nuget/v/EasyDoubles.svg)](https://www.nuget.org/packages/EasyDoubles/)
[![EasyDoubles NuGet Package Downloads](https://img.shields.io/nuget/dt/EasyDoubles)](https://www.nuget.org/packages/EasyDoubles)
[![Developed by Tigran (TIKSN) Torosyan](https://img.shields.io/badge/Developed%20by-Tigran%20%28TIKSN%29%20Torosyan-orange.svg)](https://tiksn.com/project/easydoubles/)
[![StandWithUkraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://github.com/vshymanskyy/StandWithUkraine/blob/main/docs/README.md)

# EasyDoubles - Test Doubles Framework

EasyDoubles is a lightweight and flexible test doubles framework designed to simplify the creation of mocks, stubs, and fakes for your .NET applications, with a particular focus on repository and data access layers. It aims to reduce the boilerplate often associated with manual test double creation, enabling developers to write more robust and maintainable unit and integration tests faster.

## Use Cases

-   **Unit Testing Repositories**: Easily create in-memory or fake implementations of your data repositories to isolate and test business logic without requiring a live database connection. This speeds up test execution and makes tests more reliable.
-   **Integration Testing Data Access**: For scenarios where you need more than a simple mock but still want controlled data, EasyDoubles can provide configurable fakes that simulate data access behavior, allowing for focused integration tests.
-   **Accelerating Development with Stubs**: When collaborating on projects, use EasyDoubles to stub out dependencies that are not yet implemented, allowing independent development of components that rely on them.
-   **Behavior Verification with Mocks**: Verify interactions with dependencies (e.g., ensuring a specific method was called on a repository) using built-in mocking capabilities.

EasyDoubles made to be used with [TIKSN Framework](https://github.com/tiksn/TIKSN-Framework), especially for creation of test doubles for repositories.
