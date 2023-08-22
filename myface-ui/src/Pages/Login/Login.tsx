import React, {FormEvent, useContext, useEffect, useState} from 'react';
import {Page} from "../Page/Page";
import {LoginContext} from "../../Components/LoginManager/LoginManager";
import "./Login.scss";
import { useHistory } from 'react-router-dom';

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);
    const history = useHistory();
    
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    useEffect(()=>{
        console.log(loginContext);
        if(loginContext.isLoggedIn){
            history.push("/")
        }
    }, [loginContext.isLoggedIn])
    
    function tryLogin(event: FormEvent) {
        event.preventDefault();
        loginContext.logIn(username, password);
        console.log("Completed login");
        
    }
    
    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)}/>
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)}/>
                </label>
                
                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}