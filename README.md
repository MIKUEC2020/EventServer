# EventServer

リアルタイムイベントを一斉送信するサーバー

最大3000人程度まで送信できます。

### EventServer

Redisのpub/subを監視してイベントを送信するプラグラム

### EventClient

EventServerからのイベントを受信するプログラム

### RedisPublisher

Redisのpub/subにイベントを送信するテスト用プログラム

### EventServerCoreLibrary

一定間隔で処理を行うクラスを抽象化したライブラリ
