# Increasing session conccurency per container

By default, only one session is configured to run per container through the SE_NODE_MAX_SESSIONS environment variable. It is possible to increase that number up to the maximum available processors, this is because more stability is achieved when one container/browser has 1 CPU to run.

However, if you have measured performance and based on that, you think more sessions can be executed in each container, you can override the maximum limit by setting both SE_NODE_MAX_SESSIONS to a desired number and SE_NODE_OVERRIDE_MAX_SESSIONS to true. Nevertheless, running more browser sessions than the available processors is not recommended since you will be overloading the resources.

Overriding this setting has an undesired side effect when video recording is enabled since more than one browser session might be captured in the same video.