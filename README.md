# ValidTextChecker
【Unity】InputFieldの絵文字入力防止

https://user-images.githubusercontent.com/49199105/143731730-a6d92439-1e6e-47a4-8763-4de41b476caf.mp4

InputFieldに入力される絵文字を除去するスクリプトです。<br>
InputFieldの付いたオブジェクトにValidTextCheckerコンポーネントを付けて使用します。<br>

スクリプトでは下記の入力をはじきます。<br>
※ 入力から削除までのラグのため、モバイルキーボード上の入力には一瞬映ります。<br>
・サロゲートペア(4バイト絵文字)<br>
・Textコンポーネントに設定しているフォントで空欄となる文字(3バイト絵文字)<br>
・ユーザー指定の文字<br>
