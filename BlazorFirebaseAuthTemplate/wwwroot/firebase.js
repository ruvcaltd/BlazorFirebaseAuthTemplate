// TODO: Replace the following with your app's Firebase project configuration
const firebaseConfig = {
    apiKey: "",
    authDomain: "",
    databaseURL: "",
    projectId: "",
    storageBucket: "",
    messagingSenderId: "",
    appId: ""
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

console.log("firebase.js loaded");;


window.FirebaseLogin = (instance) => {

    var provider = new firebase.auth.GoogleAuthProvider();
 
    if (localStorage.token) {
        console.log(localStorage.token);
        instance.invokeMethod('LoginCallback', localStorage.email, localStorage.display, localStorage.token);
    }
    else {
        firebase.auth().signInWithPopup(provider).then(function (result) {
            // This gives you a Google Access Token. You can use it to access the Google API.
            var token = result.credential.accessToken;
            //console.log(token);
            // The signed-in user info.
            var user = result.user;
            // ...
            console.log(user);           


            user.getIdToken(/* forceRefresh */ true).then(function (idToken) {
                
                localStorage.display = user.displayName;
                localStorage.email = user.email;
                localStorage.token = idToken;
                instance.invokeMethod('LoginCallback', user.email, user.displayName, idToken);   

            }).catch(function (error) {
                console.log(error);
            });


                                             

        }).catch(function (error) {
            // Handle Errors here.
            var errorCode = error.code;
            var errorMessage = error.message;
            // The email of the user's account used.
            var email = error.email;
            // The firebase.auth.AuthCredential type that was used.
            var credential = error.credential;
            // ...
        });
    }
};


window.FirebaseLogout = (instance) => {
    firebase.auth().signOut().then(() => {
        localStorage.display = "";
        localStorage.email = "";
        localStorage.token = "";
        instance.invokeMethod('LogoutCallback');
    });
};
