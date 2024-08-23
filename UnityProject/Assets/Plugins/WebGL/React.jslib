mergeInto(LibraryManager.library, {
  GameOverExtern: function (userName, score) {
    window.dispatchReactUnityEvent("GameOver", UTF8ToString(userName), score);
  },
	NoticesStart: function () {
    window.alert("Notices Start!!!");
  },
  OpenReactWindowNotices: function (roomName) {
    window.dispatchReactUnityEvent("OpenReactWindowNotices", UTF8ToString(roomName));
  },
});