# TJAPlayer3
DTXManiaをいじってtja再生プログラムにしちゃった[TJAPlayer2fPC](https://github.com/kairera0467/TJAP2fPC)をForkして本家風に改造したアレ。

この改造者[@aioilight_2nd](https://twitter.com/aioilight_2nd)はプログラミングが大変苦手なので、スパゲッティコードと化していると思います...すみません

完成品はこちらから入手してください。

https://aioilight.space/taiko/tjap3/

このプログラムを使用した不具合・問題については責任を負いかねます。

## How 2 Build
- VisualStudio 2017 & C# 7.3
- VC++ toolset
- SlimDX用の署名

## 実装状況いろいろ
- [x] 小さいコンボ数
- [x] 踊り子
- [x] モブ
- [x] BPMと同期した音符アニメーション
- [x] ゴーゴータイム開始時の花火
- [x] 連打時の数字アニメーション
- [x] ランナー
- [x] 10コンボごとのキャラクターアニメーション
- [x] ぷちキャラ
- [x] 段位認定（段位チャレンジ）

## ロードマップみたいな

Ver.1.4.x : 拡張性の増加、サウンド周りの変更、読み込める命令の追加（9月中）

Ver.1.5.x : 段位認定機能の追加（11、12月中）

Ver.1.6.x : 多言語対応（2月中）

Ver.1.7.x : フレームワークのアップデート、ライブラリの更新（3月9日）

Ver.1.8.x : さらなる安定化を目指して（Ver.1.x系の最終バージョン、来年中）

Ver.2.x : Direct3D11、12への対応、保守体制へ（未定）

## ライセンス関係
Fork元より引用。

> 以下のライブラリを使用しています。
> * bass
> * Bass.Net
> * DirectShowLib
> * FDK21
> * SlimDX
> * xadec
> * IPAフォント
> * libogg
> *libvorbis
> 「実行時フォルダ/Licenses」に収録しています。
> 
> また、このプログラムはFROM氏の「DTXMania」を元に製作しています。
