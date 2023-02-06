## Azure AppConfigurationの特徴
- 設定を保存するのに特化したkey-valueストア
- 任意の区切り文字？やラベルでkeyをグループ化できる
  - AppConfiguration自体に区切り文字を判別して階層化する仕組みはない
    - なのでキーごとに区切り文字を分けること自体はできる、Redisと似たような感じ
    - .net用に提供されてるライブラリだと「:」で区切る前提になってる
    - ドキュメントを見ると区切り文字の例が「/」となっていることが多いので注
  - ラベルを判断してグループ化する仕組みは（見た目だけだけど）ある
   - ラベルは、別バージョンをとるのに使う（プログラム的にどのラベルが指定されてるかは意識しないようにする感じ）
- 一定期間の履歴が保存され、ロールバックも可能
- アクセスは、パブリックorプライベートエンドポイント接続
- （ポータルの機能）ある時点とある時点の比較ができる
- 構成ファイルのエクスポート＆インポートができる
  - CI/CDで管理もできる
-  値は直接ここに保存せず、別のサービスに置いて参照先だけ書くユースケースもそうていしている
  - コンテンツの種類を使う
- 更新については自分で更新する処理を書く
  - 
    - https://learn.microsoft.com/ja-jp/azure/azure-app-configuration/enable-dynamic-configuration-dotnet-core

https://learn.microsoft.com/ja-jp/dotnet/api/Microsoft.Extensions.Configuration.AzureAppConfiguration?view=azure-dotnet
- push notificationを使って設定値更新の監視（webhook?）もできそう
  - Azure Event Gridを使用

## Azure AppConfigurationのベストプラクティス
- キャッシュさせる（ライブラリを使うと勝手にキャッシュはしてくれる）　
 - デフォルト30秒
 https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.configuration.azureappconfiguration.azureappconfigurationrefreshoptions.setcacheexpiration?view=azure-dotnet#microsoft-extensions-configuration-azureappconfiguration-azureappconfigurationrefreshoptions-setcacheexpiration(system-timespan)

- キー更新を監視する場合には、全部のキーを見るのではなく、「このキーが更新されたら全部読み込みなおす（センチネルキー）」を用意する（v1.0とかそんな感じのを書くイメージ）

## 注意点
- 大文字と小文字は区別されるが、アプリ側の仕様により結果的に区別できないことがあるため大文字小文字では区別しないほうが無難

## 感想
- keyとラベルは無秩序に作ると大変そう、あるいはAppConfigurationレベルで分けてしまう？
 - 特にラベルには複数の意味を持たせないように 