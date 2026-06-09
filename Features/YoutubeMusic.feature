Feature: YouTube Music Playback

  @Youtube
  Scenario Outline: Play songs and close browser after completion
    Given the user launches the browser
    And the user navigates to YouTube

    When the user searches for "<songName>"
    And the user opens the official song video

    Then the video should start playing
    And the video should complete playback

    Then the video should be paused
    And the browser should be minimized
    And the browser should be closed

  Examples:
    | songName                                    |
    | raise of dragon tamil video song 4k         |
    | pavazha Malli official song 4k              |
    | Va va Nilava pidichi tharava                |