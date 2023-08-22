import React, {createContext, ReactNode, useState} from "react";
import { login } from "../../Api/apiClient";

type FormStatus = "READY" | "SUBMITTING" | "ERROR" | "FINISHED"

export const LoginContext = createContext<{
    username?: string,
    password?: string,
    isLoggedIn: boolean,
    isAdmin: boolean,
    logIn: (username?: string, password?: string) => void,
    logOut: () => void,
}>({
    username: undefined,
    password: undefined,
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState<string>();
    const [password, setPassword] = useState<string>();
    const [status, setStatus] = useState<FormStatus>("READY");

    
    function logIn(username?: string, password?: string) {
        console.log(username, password);
        if (username && password) {
            login(username, password)
            .then(() => {
                console.log("success");
                setStatus("FINISHED");
                setUsername(username);
                setPassword(password);
                setLoggedIn(true); 
            })
            .catch(() => {
                console.log("failure");
                setStatus("ERROR");
                setLoggedIn(false);
                setUsername(undefined);
                setPassword(undefined);
            });
        }
    }
    
    function logOut() {
        setLoggedIn(false);
        setUsername(undefined);
        setPassword(undefined);
    }
    
    const context = {
        username: username,
        password: password,
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}