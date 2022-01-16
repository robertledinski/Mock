import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as AccountStore from '../store/AccountReducer';

type AccountProperties =
    AccountStore.AccountState &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;


class AccountData extends React.PureComponent<AccountProperties> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Account Summary</h1>
                <p>Total debits. {this.props.account.totalDebits}</p>
                <p>Total credits. {this.props.account.totalCredits}</p>
                {this.renderAccountTable()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestAccount();
    }

    private renderAccountTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Balance</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.account.endOfDayBalances.map((data: AccountStore.Balances) =>
                        <tr >
                            <td>{data.date}</td>
                            <td>{data.balance}</td>
                        </tr>
                          )}
                </tbody>
            </table>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.account, // Selects which state properties are merged into the component's props
    AccountStore.actionCreators // Selects which action creators are merged into the component's props
)(AccountData as any); // eslint-disable-line @typescript-eslint/no-explicit-any
