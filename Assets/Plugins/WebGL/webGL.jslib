mergeInto( LibraryManager.library, {
    openLoadingModal: function (){
        window.dispatchReactUnityEvent(
   	    	    "openLoadingModal"
        );
    },
    closeLoadingModal: function (){
        window.dispatchReactUnityEvent(
         	    "closeLoadingModal"
        );
    },
    okayToLeave: function (){
        window.dispatchReactUnityEvent(
         	    "okayToLeave"
        );
    },
    pingAck: function (){
         window.dispatchReactUnityEvent(
          	    "pingAck"
         );
    },
    onUnityLoaded: function (){
         window.dispatchReactUnityEvent(
          	    "onUnityLoaded"
         );
    },
    onDisconnectServer: function (){
         window.dispatchReactUnityEvent(
                "onDisconnectServer"
         );
    },
    seminarEnter: function (){
         window.dispatchReactUnityEvent(
               "seminarEnter"
         );
    },
    fullScreen: function (msg){
         window.dispatchReactUnityEvent(
               "fullScreen",
    			Pointer_stringify(msg)
         );
    },
    requestUIChange: function (type){
         window.dispatchReactUnityEvent(
              "requestUIChange",
              Pointer_stringify(type)
         );
    },
    isSeminarEnterable: function (){
         window.dispatchReactUnityEvent(
               "isSeminarEnterable"
         );
    }
});