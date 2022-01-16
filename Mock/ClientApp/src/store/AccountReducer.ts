import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';


export interface AccountState {
    isLoading: boolean;
    account: AccountSummary;
}

export interface Data {
    data: AccountSummary;
}


export interface AccountSummary {
    totalCredits: number;
    totalDebits: number;
    endOfDayBalances: Balances[];
}

export interface Balances
{
    date: string;
    balance: number;
}

interface RequestAccountAction {
    type: 'REQUEST_ACCOUNT';
}

interface RecieveAccountAction {
    type: 'RECEIVE_ACCOUNT';
    account: AccountSummary;
}

type KnownAction = RequestAccountAction | RecieveAccountAction;

export const actionCreators = {
    requestAccount: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        try {
            if (appState && appState.account ) {
                fetch(`account`)
                    .then(response => response.json() as Promise<AccountSummary>)
                    .then(data => {
                        dispatch({ type: 'RECEIVE_ACCOUNT', account: data });
                    });

                dispatch({ type: 'REQUEST_ACCOUNT' });
            }

        } catch (e) {
            debugger;
            throw e;
        }
    }
};

const unloadedState: AccountState = { isLoading: false, account: { totalCredits: 0, totalDebits: 0, endOfDayBalances: [] } };

export const reducer: Reducer<AccountState> = (state: AccountState | undefined, incomingAction: Action): AccountState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_ACCOUNT':
            return {
                account: state.account,
                isLoading: true
            };
        case 'RECEIVE_ACCOUNT':
            return {
                account: action.account,
                isLoading: false
            };
        default:
            return state
    }
}
