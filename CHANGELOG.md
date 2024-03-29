# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/) and this project adheres to Semantic Versioning.

## [Unreleased]
### Fixed
- build: update vunerable Newtonsoft.Json package to latest stable.
- Avoid memory leak by unsubscribing from `ReceivedMessageFrame` event.

## [1.0.0-alpha3] - 2018-10-11
### Added
- Add AlphaPointClient class that calls the WebSocket client.
- Add response classes for the products, instruments, and miscellaneous sections.

## [1.0.0-alpha2] - 2018-10-10
### Added
- Deserialize message frame payload into object and include classes for public responses.

## 1.0.0-alpha1 - 2018-10-09
### Added
- First alpha release. A simple wrapper around the WebSocket to send and receive the message frame.

[Unreleased]: https://github.com/RobJohnston/AlphaPoint.Api/compare/v1.0.0-alpha3...HEAD
[1.0.0-alpha3]: https://github.com/RobJohnston/AlphaPoint.Api/compare/v1.0.0-alpha2...v1.0.0-alpha3
[1.0.0-alpha2]: https://github.com/RobJohnston/AlphaPoint.Api/compare/v1.0.0-alpha1...v1.0.0-alpha2