import React, {createContext, useContext, useState, ReactNode} from "react";
import MessageModal from "@/src/components/MessageModal";
import {PrimaryButtonProps} from "@/src/components/PrimaryButton";

type ShowModalArgs = {
    title: string;
    message: string;
    buttonProps?: PrimaryButtonProps;
};

const ModalContext = createContext<{
    showModal: (args: ShowModalArgs) => void;
    hideModal: () => void;
} | null>(null);

export function ModalProvider({children}: { children: ReactNode }) {
    const [visible, setVisible] = useState(false);
    const [title, setTitle] = useState<string | undefined>(undefined);
    const [message, setMessage] = useState("");
    const [buttonProps, setButtonProps] = useState<PrimaryButtonProps | undefined>(
        undefined
    );

    const showModal = ({title, message, buttonProps}: ShowModalArgs) => {
        setTitle(title);
        setMessage(message);
        setButtonProps(buttonProps);
        setVisible(true);
    };

    const hideModal = () => {
        setVisible(false);
        setTitle(undefined);
        setMessage("");
        setButtonProps(undefined);
    };

    return (
        <ModalContext.Provider value={{showModal, hideModal}}>
            {children}
            <MessageModal
                visible={visible}
                title={title}
                message={message}
                onRequestClose={hideModal}
                buttonProps={{
                    title: "Ok",
                    onPress: hideModal,
                    ...buttonProps,
                }}
            />
        </ModalContext.Provider>
    );
}

export function useModal() {
    const ctx = useContext(ModalContext);
    if (!ctx) throw new Error("useModal must be used inside ModalProvider");
    return ctx;
}
